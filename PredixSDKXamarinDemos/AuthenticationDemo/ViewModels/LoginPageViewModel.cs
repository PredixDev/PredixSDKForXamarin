using System;
using System.Threading.Tasks;
using PredixSDKForWindows.Authentication;
using PredixSDKForWindows.Networking;

namespace AuthenticationDemo
{
	public class LoginPageViewModel
    {
        private Uri baseURL = new Uri("https://predixsdkforiosexampleuaa.predix-uaa.run.aws-usw02-pr.ice.predix.io");
        private string clientID = "NativeClient";
        private string clientSecret = "test123";
        private string serverEndPointURL = "";

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
            AuthenticationConfiguration authConfig = new AuthenticationConfiguration(baseURL, clientID, clientSecret);

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
