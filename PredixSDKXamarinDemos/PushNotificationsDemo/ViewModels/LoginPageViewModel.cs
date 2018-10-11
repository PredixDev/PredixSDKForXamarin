using System;
using System.Threading.Tasks;
using PredixSDKForWindows.Authentication;
using PredixSDKForWindows.Networking;
using Toolbox.Portable;
using Xamarin.Forms;

namespace PushNotificationsDemo
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string _notificationMessage;

        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set { RaiseAndUpdate(ref _notificationMessage, value); }
        }

        public LoginPageViewModel()
        {
            MessagingCenter.Subscribe<object, string>(this, App.NotificationReceivedKey, (sender, message) =>
            {
                NotificationMessage = message;
            });
        }

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
                AuthorizationHandler = new UaaAuthorizationHandler(),
                Reachability = new Reachability()
            };

            // Tell authentication manager we are ready to authenticate, 
            // once we call authenticate it will call the authentication handler with the credential provider
            return await authenticationManager.Authenticate();
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
    }
}
