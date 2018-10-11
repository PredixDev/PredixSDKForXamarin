using System;
namespace PushNotificationsDemo
{
    public static class Constants
    {
        public static Uri PushServiceURL = new Uri("https://push-notification.run.aws-usw02-dev.ice.predix.io/notification/register");
        public static string AndroidAppKey = "5e25e162-f0b6-4eea-9ce2-ccc7fd40e246";
        public static string IOSAppKey = "21ddbcd0-9279-48bd-8784-50234b6a3820";

        public static Uri UaaServer = new Uri("https://39c2c0ae-6e65-4f38-adfc-349d158ecc4d.predix-uaa.run.aws-usw02-pr.ice.predix.io");
        public static Uri RedirectUri = new Uri("predixsdk://predixsdkforios.io/authorization_grant");

        public static string ClientID = "login_client_id";
        public static string ClientSecret = "secret";
    }
}
