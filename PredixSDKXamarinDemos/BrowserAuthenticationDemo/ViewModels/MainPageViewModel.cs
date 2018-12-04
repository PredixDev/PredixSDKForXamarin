using System;
using System.Threading.Tasks;
using PredixSDKForXamarin.Authentication;
using PredixSDKForXamarin.Networking;
using Toolbox.Portable;

namespace BrowserAuthenticationDemo
{
    public class MainPageViewModel : BaseViewModel
    {
        private bool _isAuthenticating;
        private string _authenticationStatus;

        #region Properties

        public bool IsAuthenticating
        { 
            get { return _isAuthenticating; }
            set { RaiseAndUpdate(ref _isAuthenticating, value); }
        }

        public string AuthenticationStatus
        {
            get { return _authenticationStatus; }
            set { RaiseAndUpdate(ref _authenticationStatus, value); }
        }

        #endregion

        public MainPageViewModel()
        {
            AuthenticationStatus = "Authentication Status:\nOffline";
            IsAuthenticating = false;
        }

        public async Task SetupLogin(IBrowserAuthenticationDelegate browserDelegate)
        {
            AuthenticationStatus = "Authentication Status:\nAuthenticating...";
            IsAuthenticating = true;

            // We'll use the browserDelegate for handling events.
            //
            // create an AuthenticationManagerConfiguration, and load associated 
            // UAA endpoint from our Predix Mobile server endpoint
            AuthenticationConfiguration authConfig = new AuthenticationConfiguration(Constants.UaaServer, Constants.ClientID, Constants.ClientSecret);

            // Create an OnlineAuthenticationHandler object -- in this case our online handler subclass is designed to work with a UAA login web page.
            // The redirectURI is setup in the UAA configuration
            IAuthenticationHandler onlineAuthHandler = new UaaBrowserAuthenticationHandler(Constants.RedirectUri, browserDelegate);

            // Create our AuthenticationManager with our configuration
            AuthenticationManager authenticationManager = new AuthenticationManager(authConfig, onlineAuthHandler)
            {
                //associate an AuthorizationHandler with the AuthenticationManager
                AuthorizationHandler = new UaaAuthorizationHandler(),
                Reachability = new Reachability()
            };

            var result = await authenticationManager.Authenticate();
            Console.WriteLine("Authentication Completed");

            AuthenticationStatus = $"Authentication Status:\nSuccess - {result.Successful}\nError - {result.Error?.ToString() ?? "None"}";
            IsAuthenticating = false;
        }
    }
}

