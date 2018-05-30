using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using PredixSDKForWindows.Storage;
using Xamarin.Forms;

namespace DatabaseDemo
{
    public class MainPageViewModel
    {
        private Command _openDatabaseCommand;
        private Command _addDocumentsCommand;

		private Database.Configuration _config;
        private Database _database;
        private List<string> _fruitList = new List<string>() { "Apple", "Papaya", "Peach", "Pitaya", "Passion fruit", "Banana", "Pear", "Mango", "Cherry", "Plum", "Apricot", "Lemon", "Avocado", "Fig", "Lychee", "Coconut", "Cantaloupe", "Tangerine", "Clementine", "Pineapple", "Grape", "Grapefruit", "Pomelo", "Orange", "Date palm", "Watermelon", "Kumquat", "Breadfruit", "Blueberry", "Honeydew", "Lime", "Raspberry", "Strawberry", "Tomato", "Guava", "Kiwi" };

        public Command OpenDatabaseCommand => _openDatabaseCommand ?? (_openDatabaseCommand = new Command(OpenDatabase));
        public Command AddDocumentsCommand => _addDocumentsCommand ?? (_addDocumentsCommand = new Command(AddDocuments));
        
        private void OpenDatabase()
        {
			_config = Database.Configuration.GetDefaultConfiguration();
			Database.Delete(_config);
			_database = Database.Open(create: true);
        }

        private void AddDocuments()
        {
            _fruitList.Sort();

            for (int a = 0; a < _fruitList.Count; a++)
            {
				Document document = new Document(a.ToString());
                document.Properties["fruit"] = _fruitList[a];
                _database.Add(document);
            }
        }

        public ObservableCollection<FruitDocument> FetchDocuments()
        {
            var documents = new ObservableCollection<FruitDocument>();

            for (int a = 0; a < _fruitList.Count; a++)
            {

                UpdateResult result = _database.FetchDocument(a.ToString());
				var propertiesJson = JsonConvert.SerializeObject(result.Document.ToDictionary());
                var documentData = JsonConvert.DeserializeObject<FruitDocument>(propertiesJson);
                documents.Add(documentData);
            }

            return documents;
        }

    }
}