using System;

namespace BrowserAuthenticationDemo
{
    internal class Constants
    {
        public static Uri UaaServer = new Uri("https://predixsdkforiosexampleuaa.predix-uaa.run.aws-usw02-pr.ice.predix.io");
        public static Uri RedirectUri = new Uri("predixsdk://predixsdkforios.io/authorization_grant");

        public static string ClientID = "NativeClient";
        public static string ClientSecret = "test123";
    }
}
