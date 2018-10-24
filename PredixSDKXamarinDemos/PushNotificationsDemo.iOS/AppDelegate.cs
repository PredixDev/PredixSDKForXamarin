using System;

using Foundation;
using UIKit;
using UserNotifications;
using Toolbox.Portable;
using PredixSDKForWindows.Managers.PushNotifications;
using Xamarin.Forms;

namespace PushNotificationsDemo.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // Register to receive remote alert and badge notification. Learn more about Xamarin iOS push notifications here. 
            // https://docs.microsoft.com/en-us/azure/notification-hubs/xamarin-notification-hubs-ios-push-notification-apns-get-started
            //
            // Note: According to Apple, only 5% of device run iOS 10 or older versions (as of September 2018). 
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
                {
                    // If authorized, we can assume we have already registered for push notifications.
                    if (settings.AuthorizationStatus != UNAuthorizationStatus.Authorized) 
                    {
                        // We have not registered our device for push notification. Request authorization from user.
                        UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Sound, (granted, error) =>
                        {
                            if (granted)
                                InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                        });
                    }
                });
            }

            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

        /// Called when finished registering for push notification with APNS (NOT the mobile push notification service).
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var deviceId = UIDevice.CurrentDevice.IdentifierForVendor.ToString();
            var token = ToHexEncodedString(deviceToken.ToArray());
            ServiceContainer.Register<IPushNotificationRegistration>(() => new PushNotificationRegistration(token, deviceId, Constants.IOSAppKey));
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string message = string.Empty;

                //Extract the alert text
                // NOTE: If you're using the simple alert by just specifying
                // "  aps:{alert:"alert msg here"}  ", this will work fine.
                // But if you're using a complex alert with Localization keys, etc.,
                // your "alert" object from the aps dictionary will be another NSDictionary.
                // Basically the JSON gets dumped right into a NSDictionary,
                // so keep that in mind.
                if (aps.ContainsKey(new NSString("alert")))
                    message = (aps[new NSString("alert")] as NSString).ToString();

                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(message))
                    {
                        MessagingCenter.Send<object, string>(this, App.NotificationReceivedKey, message);
                    }
                }
            }
        }

        // Helper function to convert bytes into a hex string
        string ToHexEncodedString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
