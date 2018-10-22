using System;
namespace PushNotificationsDemo
{
    public static class Constants
    {
        public static Uri PushServiceURL = new Uri("https://push-notification.run.aws-usw02-dev.ice.predix.io/notification/register");
        public static string AndroidAppKey = "9881c302-de60-452e-820b-058ebcc8d4d3";
        public static string IOSAppKey = "c5252da3-0364-4fd3-ae98-c9365fde4f27";

        public static Uri UaaServer = new Uri("https://39c2c0ae-6e65-4f38-adfc-349d158ecc4d.predix-uaa.run.aws-usw02-pr.ice.predix.io");
        public static Uri RedirectUri = new Uri("predixsdk://predixsdkforios.io/authorization_grant");

        public static string ClientID = "login_client_id";
        public static string ClientSecret = "secret";
    }
}
