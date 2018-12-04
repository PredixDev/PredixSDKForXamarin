using System;
using System.Collections.Generic;
using PredixSDKForXamarin.Authentication;
using Xamarin.Auth;
using Xamarin.Forms;

namespace BrowserAuthenticationDemo
{
    public partial class LoginPage : ContentPage, IBrowserAuthenticationDelegate
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();

            LoginButton.Clicked += AuthButton_ClickedAsync;
        }

        async void AuthButton_ClickedAsync(object s, EventArgs e)
        {
            var viewModel = (MainPageViewModel)BindingContext;
            await viewModel.SetupLogin(this);
        }

        // In this case when we have a login URL to display. We'll use Xamarin.Auth display the login URL.
        // and a WebRedirectAuthenticator class to interept redirects.
        public void LoadAuthenticationuri(IBrowserRedirectReceiver redirectReceiver, Uri uri)
        {
            var authenticator = new BrowserWebAuthenticator(uri, Constants.RedirectUri);
            authenticator.Completed += (sender, eventArgs) =>
            {
                redirectReceiver.ProcessUri(authenticator.RedirectUrl);
            };

            authenticator.Error += (object sender, AuthenticatorErrorEventArgs e) => 
            {
                Console.WriteLine($"Error: {e}");
            };

            AuthenticationState.Authenticator = authenticator;

            // With Xamarin.Auth, the native presenters (Login UI) is initialized in 
            // the platform specific projects. 
            //
            // See below for details on using Xamarin.Auth
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

    }

    /// <summary>
    /// Browser web authenticator.
    /// 
    /// You may choose to implement Browser auth differently (or manually), but for an understanding on
    /// Xamarin.Auth check out this Microsoft doc: <see cref="https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/authentication/oauth"/>
    /// 
    /// Instead of using the Oauth2Authenticator class, we're deriving it's superclass (WebRedirectAuthenticator)
    /// here to handle directly what happens on a redirect. This allows us to 'Succeed' on retrieving the authorization code
    /// and not the access token.
    /// 
    /// </summary>
    public class BrowserWebAuthenticator : WebRedirectAuthenticator
    {
        public Uri RedirectUrl { get; private set; }

        public BrowserWebAuthenticator(Uri initialUrl, Uri redirectUrl) : base(initialUrl, redirectUrl)
        {
        }

        /// <summary>
        /// On the redirect page loaded.
        /// 
        /// This method is called once our redirect page (Constants.RedirectURI) has been loaded.
        /// </summary>
        protected override void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            RedirectUrl = url;
            OnSucceeded(username: "N/A", accountProperties: null);
        }
    }
}
