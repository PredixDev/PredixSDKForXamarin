using System;
using Serilog;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ReplicationDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new ReplicationDemoPage());
            Log.Information("SERILOGGGGGGGGGGGGGGGGGGGGGGGGGGGGG");
            Log.Debug("DEBUGGGGGGGGGGGGGGGGGGGGGGG");
            Log.Error("ERRRRROOOOORRRRRRRRR");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
