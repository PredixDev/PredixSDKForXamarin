using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PredixSDKForWindows.Storage;
using Xamarin.Forms;

namespace DatabaseDemo
{
    public class MainPageViewModel
    {
        private Command _openDatabaseCommand;
        private Command _addDocumentsCommand;

		private Database.OpenDatabaseConfiguration _config;
        private Database _database;

        public Command OpenDatabaseCommand => _openDatabaseCommand ?? (_openDatabaseCommand = new Command(OpenDatabase));
        public Command AddDocumentsCommand => _addDocumentsCommand ?? (_addDocumentsCommand = new Command(AddDocuments));

		private List<string> _fruitList = new List<string>() { "Apple", "Papaya", "Peach", "Pitaya", "Passion fruit", "Banana", "Pear", "Mango", "Cherry", "Plum", "Apricot", "Lemon", "Avocado", "Fig", "Lychee", "Coconut", "Cantaloupe", "Tangerine", "Clementine", "Pineapple", "Grape", "Grapefruit", "Pomelo", "Orange", "Date palm", "Watermelon", "Kumquat", "Breadfruit", "Blueberry", "Honeydew", "Lime", "Raspberry", "Strawberry", "Tomato", "Guava", "Kiwi" };
		private string fruitNameKey = "fruit";
		private string notesKey = "notes";

        private void OpenDatabase()
        {
			try 
			{
				// Open the database
				_config = Database.OpenDatabaseConfiguration.Default;
				_database = Database.Open(configuration: _config, create: true);
			}
			catch (Exception error)
			{
				Console.WriteLine($"Error opening database: {error}");
			}
        }

        private void AddDocuments()
        {         
            // see if we have document "0", if so we've already created the sample data
            _database.FetchDocument("0", (document) =>
            {
				if (document == null)
                {
					CreateSampleDocuments();
                }
            });
        }

		private void CreateSampleDocuments()
        {
            _fruitList.Sort();

            var fruitsAdded = 0;

            for (int rowIndex = 0; rowIndex < _fruitList.Count; rowIndex++)
            {
                var item = _fruitList[rowIndex];

                // create a simple document.
                // The id of the document will be the row number,
                // the only data in the document will be the fruit name, and an empty "notes" fielda.
                Document document = new Document(rowIndex.ToString())
                {
                    Properties = new Dictionary<string, object> { { "id", rowIndex }, { fruitNameKey, item } }
                };

                // An alternative method of adding new properties
                document.Properties[notesKey] = "";

                // add the document to the database
				_database.Add(document, (_) =>
                {
                    fruitsAdded += 1;
                    if (fruitsAdded >= _fruitList.Count)
                    {
                        Console.WriteLine($"Added all {fruitsAdded} fruits!");
                    }
                });
            }
        }

		public ObservableCollection<FruitDocument> FetchDocuments()
        {
            var documents = new ObservableCollection<FruitDocument>();
			var tasks = new Task[_fruitList.Count];

			// Fetch documents from the database
			for (int i = 0; i < _fruitList.Count; i++)
            {

				tasks[i] = _database.FetchDocument(i.ToString(), (result) =>
				{
					var propertiesJson = JsonConvert.SerializeObject(result.ToDictionary());
                    var documentData = JsonConvert.DeserializeObject<FruitDocument>(propertiesJson);
                    documents.Add(documentData);	
				});
            }

			// Ensure each document is fetched before returning the documents
			Task.WaitAll(tasks);
            return documents;
        }
    }
}