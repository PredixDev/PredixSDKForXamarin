using Xamarin.Forms;
using PredixSDKForWindows.Managers.PushNotifications;
using Toolbox.Portable;

namespace PushNotificationsDemo
{
    public partial class LoginPage : ContentPage
    {
        private IPushNotificationRegistration _pushNotificationService;

        public LoginPage()
        {
            InitializeComponent();
            LoginButton.Clicked += LoginButton_Clicked;
            BindingContext = new LoginPageViewModel();
        }

        async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            LoginProgressIndicator.IsRunning = true;
            var viewModel = (LoginPageViewModel)BindingContext;
            var result = await viewModel.PerformLoginAsync(UsernameField.Text, PasswordField.Text);

            if (result.Successful)
            {
                StatusLabel.Text = "Authenticated. Registering for Push Notifications..";

                _pushNotificationService = ServiceContainer.Resolve<IPushNotificationRegistration>(nullIsAcceptable: true);
                if (_pushNotificationService != null)
                {
                    var error = await _pushNotificationService.RegisterAsync(Constants.PushServiceURL, UsernameField.Text);
                    if (error != null)
                    {
                        LoginProgressIndicator.IsRunning = false;
                        StatusLabel.Text = $"Error In Registration: {error}";
                        System.Diagnostics.Debug.WriteLine($"RegisterNativeAsync error: {error}");
                    }
                    else
                    {
                        StatusLabel.Text = "Successfully registered! Try sending a message from the Predix Mobile console";
                        LoginProgressIndicator.IsRunning = false;
                    }
                }
                else
                {
                    LoginProgressIndicator.IsRunning = false;
                    StatusLabel.Text = "Either already registered or running into networking issues. Try sending a message from the Predix Mobile console";
                }
            }
            else
                await DisplayAlert("Error", "Error Logging In", "Dismiss");
        }
    }
}
