using System;
namespace PushNotificationsDemo
{
    public static class Constants
    {
        public static Uri UaaServer = new Uri("https://predixsdkpushnotificationexampleuaa.predix-uaa.run.aws-usw02-pr.ice.predix.io");
        public static Uri PushServiceURL = new Uri("https://push-notification.run.aws-usw02-dev.ice.predix.io/notification/register");

        public static string AndroidAppKey = "9881c302-de60-452e-820b-058ebcc8d4d3";
        public static string IOSAppKey = "8c8d2574-ada8-4134-96b7-99aa6ecc3762";

        public static string ClientID = "login_client_id";
        public static string ClientSecret = "secret";
    }
}
