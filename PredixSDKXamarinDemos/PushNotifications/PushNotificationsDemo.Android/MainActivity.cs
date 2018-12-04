using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Gms.Common;
using Firebase.Iid;
using Android.Util;
using Firebase.Messaging;
using Xamarin.Forms;
using Firebase;
using System.Threading.Tasks;
using Toolbox.Portable;
using PredixSDKForXamarin.Managers.PushNotifications;

namespace PushNotificationsDemo.Droid
{
    [Activity(Label = "PushNotificationsDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            // Register to receive remote alert and badge notification. 
            // Learn more about Xamarin Android push notification here:
            // https://docs.microsoft.com/en-us/azure/notification-hubs/xamarin-notification-hubs-push-notifications-android-gcm

            FirebaseApp.InitializeApp(this);
            CreateNotificationChannel();

            IsPlayServicesAvailable();

#if DEBUG
            Task.Run(() =>
            {
                FirebaseInstanceId.Instance.DeleteInstanceId();
                Console.WriteLine("Forced token: " + FirebaseInstanceId.Instance.Token);
            });
#endif
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Console.WriteLine($"Error: {GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
                else
                {
                    Console.WriteLine("This device is not supported");
                    Finish();
                }
                return false;
            }
            else
            {
                Console.WriteLine("Google Play Services is available.");
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        #region Services

        [Service]
        [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
        public class MyFirebaseIIDService : FirebaseInstanceIdService
        {
            const string TAG = "MyFirebaseIIDService";
            public override void OnTokenRefresh()
            {
                var refreshedToken = FirebaseInstanceId.Instance.Token;
                Console.WriteLine("Refreshed token: " + refreshedToken);
                Log.Debug(TAG, "Refreshed token: " + refreshedToken);

                var deviceId = FirebaseInstanceId.Instance.Id;
                var token = refreshedToken;
                ServiceContainer.Register<IPushNotificationRegistration>(() => new PushNotificationRegistration(token, deviceId, Constants.AndroidAppKey));
            }
        }

        [Service]
        [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
        public class MyFirebaseMessagingService : FirebaseMessagingService
        {
            const string TAG = "MyFirebaseMsgService";
            public override void OnMessageReceived(RemoteMessage message)
            {
                var msg = "";
                var notification = message.GetNotification();
                Log.Debug(TAG, "From: " + message.From);

                if (notification != null)
                    msg = notification.Body;
                else
                    msg = message.Data.ToString();

                Log.Debug(TAG, "Notification Message: " + msg);
                MessagingCenter.Send<object, string>(this, App.NotificationReceivedKey, msg);
            }
        }

        #endregion
    }
}