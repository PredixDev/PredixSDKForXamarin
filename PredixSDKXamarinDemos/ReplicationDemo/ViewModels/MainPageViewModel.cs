using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PredixSDKForWindows.Authentication;
using PredixSDKForWindows.Networking;
using PredixSDKForWindows.Storage;
using Xamarin.Forms;

namespace ReplicationDemo
{
    public class MainPageViewModel : ReplicationStatusDelegate
    {
        private Uri _syncUri = new Uri("https://predixsdkforiosexamplesync.run.aws-usw02-pr.ice.predix.io");
        private Uri _baseUri = new Uri("https://predixsdkforiosexampleuaa.predix-uaa.run.aws-usw02-pr.ice.predix.io");
        private string clientID = "NativeClient";
        private string clientSecret = "test123";

        private Database.ReplicationConfiguration _replicationConfig;
        private Database.OpenDatabaseConfiguration _databaseConfig;
        private Database _database;

        private Label _statusLabel;
        private Button _viewDataButton;
        private string _statusText;
        private int _rowCount;

        public MainPageViewModel(Label statusLabel, Button viewDataButton)
        {
            _statusLabel = statusLabel;
            _viewDataButton = viewDataButton;
        }

        #region Authentication Logic

        /// <summary>
        /// Performs the login async
        /// 
        /// Note: You may have to update your iOS/Android/UWP projects to use the 
        /// latest version of TLS and HttpClient if login seems to fail.
        /// 
        /// More details can be found here: 
        /// https://docs.microsoft.com/en-us/xamarin/cross-platform/app-fundamentals/transport-layer-security
        /// </summary>
        /// 
        public async Task<AuthenticationResult> PerformLoginAsync(string username, string password)
        {
            // Creates an authentication manager configuration configured for your UAA instance.  
            // The baseURL, clientId and clientSecret can also be defined in your info.plist 
            // if you wish but for simplicity they're added here.
            AuthenticationConfiguration authConfig = new AuthenticationConfiguration(_baseUri, clientID, clientSecret);

            // Give the username and password to the credential provider
            IClientCredentialProvider credentialProvider = new PredixCredentialProvider(username, password);

            // Create an online handler so that we can tell the authentication manager we want to authenticate online
            IAuthenticationHandler onlineAuthHandler = new UaaPasswordGrantAuthenticationHandler(credentialProvider);

            // Create an authentication manager with our UAA configuration, set UAA as our authorization source, 
            // set the online handler so that the manager knows we want to autenticate online
            AuthenticationManager authenticationManager = new AuthenticationManager(authConfig, onlineAuthHandler)
            {
                AuthorizationHandler = new PredixSyncAuthorizationHandler(_syncUri),
                Reachability = new Reachability()
            };

            // Tell authentication manager we are ready to authenticate, 
            // once we call authenticate it will call the authentication handler with the credential provider
            var authenticationResult = await authenticationManager.Authenticate();

            if (authenticationResult.Successful)
            {
                // Open database then begin replication
                await OpenDatabase();
                StartReplication();
            }

            return authenticationResult;
        }


        internal class PredixCredentialProvider : IClientCredentialProvider
        {
            private readonly string _username;
            private readonly string _password;

            public PredixCredentialProvider(string username, string password)
            {
                _username = username;
                _password = password;
            }

            public void ProvideCredentials(IClientCredentialReceiver receiver)
            {
                receiver.ReceiveCredentials(_username, _password);
            }
        }

        #endregion


        #region Database Logic

        private async Task OpenDatabase()
        {
            try
            {
                // Clear database so we can start from fresh, then open the database
                _databaseConfig = Database.OpenDatabaseConfiguration.Default;
                await Database.Delete(_databaseConfig);
                _database = Database.Open(configuration: _databaseConfig, create: true);

                // set this to replication delegate, so we can show replication status information
                // see ReplicationStatusDelegate extension below for these methods.
                _database.ReplicationStatusDelegate = this;

            }
            catch (Exception error)
            {
                Console.WriteLine($"Error opening database: {error}");
            }
        }

        public ObservableCollection<FruitDocument> FetchDocuments()
        {
            var documents = new ObservableCollection<FruitDocument>();
            var tasks = new Task[_rowCount];

            // Fetch documents from the database
            for (int i = 0; i < _rowCount; i++)
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

        #endregion


        #region Replication Logic

        public void StartReplication()
        {
            if (_syncUri == null)
                return;

            // start replication, for this example we're doing a non-repeating, one-direction replication.
            Console.WriteLine("Starting replication...");

            _replicationConfig = Database.ReplicationConfiguration.OneTimeServerToClientReplication(_syncUri);
            _database.StartReplication(_replicationConfig);

            var cookies = PredixHttpClient.CookieHandler.Cookies;
            foreach (var cookie in cookies)
                Console.WriteLine($"Cookie: {cookie}");

            // See ReplicationStatusDelegate methods below to follow the logic after replication starts
        }


        // These methods handle the ReplicationStatus 
        // providing information for the replication process

        // Since this replication is one-direction, we won't have any "sending"
        public void ReplicationIsSending(Database database, ReplicationDetails details, int sent, int totalToSend)
        {
        }

        // This method is called as each batch of data is received.
        public void ReplicationIsReceiving(Database database, ReplicationDetails details, int received, int totalToReceive)
        {
            _rowCount = totalToReceive - 1;
            _statusLabel.Text = $"Receiving Documents: {received} : {totalToReceive}";

            Console.WriteLine(_statusText);
        }

        // Replication failure. The Error object will contain details as to the error
        public void ReplicationFailed(Database database, ReplicationDetails details, Exception error)
        {
            _rowCount = 0;
            _statusLabel.Text = $"Replication Failed: {error}";

            Console.WriteLine(_statusText);
        }

        // Called replication is complete, even if replication failed.
        public void ReplicationDidComplete(Database database, ReplicationDetails details)
        {
            if (_rowCount > 0)
            {
                _statusLabel.Text = "Replication Completed";
                _viewDataButton.IsEnabled = true;
            }

            Console.WriteLine($"ReplicationDidComplete: rows: {_rowCount}");
        }

        #endregion


    }
}
