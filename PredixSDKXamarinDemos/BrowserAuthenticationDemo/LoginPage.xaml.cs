using System;
using System.Collections.Generic;
using Xamarin.Auth;
using Xamarin.Forms;
using PredixSDKForWindows.Authentication;

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
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

    }

    public class BrowserWebAuthenticator : WebRedirectAuthenticator
    {
        public Uri RedirectUrl { get; private set; }

        public BrowserWebAuthenticator(Uri initialUrl, Uri redirectUrl) : base(initialUrl, redirectUrl)
        {
        }

        protected override void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            RedirectUrl = url;
            OnSucceeded(username: "N/A", accountProperties: null);
        }
    }
}
