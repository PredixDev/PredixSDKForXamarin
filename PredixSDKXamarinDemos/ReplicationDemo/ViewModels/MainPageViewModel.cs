using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PredixSDKForWindows.Authentication;
using PredixSDKForWindows.Networking;
using PredixSDKForWindows.Storage;
using Toolbox.Portable;

namespace ReplicationDemo
{
    public class MainPageViewModel : BaseViewModel, IReplicationStatusDelegate
    {
        private Database.ReplicationConfiguration _replicationConfig;
        private Database.OpenDatabaseConfiguration _databaseConfig;
        private Database _database;

        private string _status;
        private bool _canViewData;
        private int _rowCount;

        #region Properties

        public string Status 
        {
            get { return _status; }
            set { RaiseAndUpdate(ref _status, value); }
        }

        public bool CanViewData
        {
            get { return _canViewData; }
            set { RaiseAndUpdate(ref _canViewData, value); }
        }

        #endregion

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
            AuthenticationConfiguration authConfig = new AuthenticationConfiguration(Constants.UaaServer, Constants.ClientID, Constants.ClientSecret);

            // Give the username and password to the credential provider
            IClientCredentialProvider credentialProvider = new PredixCredentialProvider(username, password);

            // Create an online handler so that we can tell the authentication manager we want to authenticate online
            IAuthenticationHandler onlineAuthHandler = new UaaPasswordGrantAuthenticationHandler(credentialProvider);

            // Create an authentication manager with our UAA configuration, set UAA as our authorization source, 
            // set the online handler so that the manager knows we want to autenticate online
            AuthenticationManager authenticationManager = new AuthenticationManager(authConfig, onlineAuthHandler)
            {
                AuthorizationHandler = new PredixSyncAuthorizationHandler(Constants.SyncUri),
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
                _databaseConfig = Database.OpenDatabaseConfiguration.Default;

                // Clear database so we can start from fresh, then open the database
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

        public async Task<ObservableCollection<ReplicationDataItem>> FetchDocumentsAsync()
        {
            var documents = new ObservableCollection<ReplicationDataItem>();

            // Fetch documents from the database
            for (int i = 0; i < _rowCount; i++)
            {

                var result = await _database.FetchDocument(i.ToString());
                var documentData = JsonConvert.SerializeObject(result.ToDictionary());
                var attachmentData = result.Attachments[0].Data;

                FruitDocument document = JsonConvert.DeserializeObject<FruitDocument>(documentData);
                FruitAttachment attachment = new FruitAttachment() { imageData = attachmentData.ToArray() };

                documents.Add(new ReplicationDataItem() { Document = document, Attachment = attachment });
            }

            // Ensure each document is fetched before returning the documents
            return documents;
        }

        #endregion


        #region Replication Logic

        public void StartReplication()
        {
            if (Constants.SyncUri == null)
                return;

            // start replication, for this example we're doing a non-repeating, one-direction replication.
            Console.WriteLine("Starting replication...");

            _replicationConfig = Database.ReplicationConfiguration.OneTimeServerToClientReplication(Constants.SyncUri);
            _database.StartReplication(_replicationConfig);

            var cookies = PredixHttpClient.CookieHandler.Cookies;
            foreach (var cookie in cookies)
                Console.WriteLine($"Cookie: {cookie}");

            // See ReplicationStatusDelegate methods below to follow the logic after replication starts
        }


        // These methods handle the ReplicationStatus 
        // providing information for the replication process

        // Since this replication is one-direction, we won't have any "sending"
        public void ReplicationIsSending(Database database, IReplicationDetails details, int sent, int totalToSend)
        {
        }

        // This method is called as each batch of data is received.
        public void ReplicationIsReceiving(Database database, IReplicationDetails details, int received, int totalToReceive)
        {
            _rowCount = totalToReceive - 1;
            Status = $"Receiving Documents: {received} : {totalToReceive}";

            Console.WriteLine(_status);
        }

        // Replication failure. The Error object will contain details as to the error
        public void ReplicationFailed(Database database, IReplicationDetails details, Exception error)
        {
            _rowCount = 0;
            Status = $"Replication Failed: {error}";

            Console.WriteLine(_status);
        }

        // Called when replication is complete, even if replication failed.
        public void ReplicationDidComplete(Database database, IReplicationDetails details)
        {
            if (_rowCount > 0)
            {
                Status = "Replication Completed";
                CanViewData = true;
            }

            Console.WriteLine($"ReplicationDidComplete: rows: {_rowCount}");
        }

        #endregion


    }
}
