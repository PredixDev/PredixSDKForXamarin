using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PredixSDKForWindows.Storage;
using Toolbox.Portable;
using System.Linq;

namespace IndexAndQueryDemo
{
    public class QueryListPageViewModel : BaseViewModel
    {
        string _indexName = "wordIndex";
        string _wordKey = "word";
        string _definitionKey = "definition";

        ObservableCollection<WordItem> _queryResultsData;
        bool _loadingResults = false;
        bool _resultsReady = false;

        Database.OpenDatabaseConfiguration _databaseConfig;
        Database _database;

        #region Properties

        public ObservableCollection<WordItem> QueryResultsData
        {
            get { return _queryResultsData; }
            set { RaiseAndUpdate(ref _queryResultsData, value); }
        }

        public bool LoadingResults
        {
            get { return _loadingResults; }
            set { RaiseAndUpdate(ref _loadingResults, value); }
        }

        public bool ResultsReady
        {
            get { return _resultsReady; }
            set { RaiseAndUpdate(ref _resultsReady, value); }
        }

        #endregion

        #region Public Methods

        public async Task CreateSampleDocumentsIfNeededAsync(Stream stream)
        {
            LoadingResults = true;

            // First, open the database
            OpenDatabase();

            if (_database != null)
            {
                // search for the word "a", which we know is in the dictionary....
                IQueryParameters query = new QueryByKeyList(keys: new List<object> { "a" });

                var queryResult = await _database.RunQuery(index: _indexName, parameters: query);
                if (queryResult.Count == 0)
                {
                    // No results means the inital database for this demo has not been created, so sample data will now be created.
                    await CreateSampleDocumentsAsync(stream);
                }

                // The database has been loaded, so we'll continue with page initialization
                ResultsReady = true;
                await QueryDatabaseAsync("");
            }
            else
            {
                Console.WriteLine("Error: Database was not created succssfully");
            }

            LoadingResults = false;
        }

        public async Task QueryDatabaseAsync(string searchText)
        {
            // Query the index by range. This will find all keys between the start and end keys, inclusive.
            string endKey = CalculateEndKey(searchText);
            var query = new QueryByKeyRange(startkey: searchText, endKey: endKey);

            var queryResult = await _database.RunQuery(index: _indexName, parameters: query);
            ProcessResults(queryResult);
        }

        #endregion

        void OpenDatabase()
        {
            try
            {
                // Create our index 
                var index = new Database.Index(name: _indexName, version: "1.0", map: (document, addIndexRow) =>
                {
                    // This index will have a row for each document, keyed by the "word", and having a value of the "definition"
                    if (document.Properties.TryGetValue(_wordKey, out object word))
                    {
                        addIndexRow(word, document.Properties[_definitionKey]);
                    }
                });

                // Creates a dabase configuration with default name and location, and the provided index(s)
                _databaseConfig = new Database.OpenDatabaseConfiguration(indexes: new List<IIndexer> { index });

                // Open the database
                _database = Database.Open(configuration: _databaseConfig, create: true);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error opening database: {error}");
            }
        }

        #region Private methods

        async Task CreateSampleDocumentsAsync(Stream stream)
        {
            var rowsAdded = 0;
            Dictionary<string, string> sampleData;
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                sampleData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            foreach (var entry in sampleData)
            {
                // create a simple document.
                // The id of the document will be automatically generated
                // the only data in the document will be the word and the definition
                var document = new Document(new Dictionary<string, object> { { _wordKey, entry.Key }, { _definitionKey, entry.Value } });

                // add the document to the database
                await _database.Add(document, (updateResult) => { rowsAdded += 1; });
            }
        }
        
        void ProcessResults(QueryResultEnumerator results)
        {
            Console.WriteLine($"Found {results.Count} words");
            var newWordCollection = new ObservableCollection<WordItem>();

            foreach (var row in results)
            {
                newWordCollection.Add(new WordItem()
                {
                    Word = row.Key as string,
                    Definition = row.Value as string
                });
            }
            
            QueryResultsData = newWordCollection;
        }

        string CalculateEndKey(string startKey)
        {
            if (string.IsNullOrEmpty(startKey))
                return null;

            return startKey.Remove(startKey.Length - 1) + "z";
        }

        #endregion

    }
}
