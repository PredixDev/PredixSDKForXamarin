using Xamarin.Auth;

namespace BrowserAuthenticationDemo
{
    /// <summary>
    /// Used for accessing our WebDirectAuthenticator in the platform projects
    /// </summary>
    public class AuthenticationState
    {
        public static WebRedirectAuthenticator Authenticator;
    }
}
