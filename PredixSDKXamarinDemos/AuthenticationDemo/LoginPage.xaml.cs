using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AuthenticationDemo
{
	public partial class LoginPage : ContentPage
    {
        private OnlineAPIPage _onlineAPIPage;
        public LoginPage()
        {
            InitializeComponent();
            LoginButton.Clicked += LoginButton_Clicked;
            BindingContext = new LoginPageViewModel();
        }

        async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            if (_onlineAPIPage == null)
                _onlineAPIPage = new OnlineAPIPage();

            LoginProgressIndicator.IsRunning = true;

            var viewModel = (LoginPageViewModel)BindingContext;
            var result = await viewModel.PerformLoginAsync(UsernameField.Text, PasswordField.Text);
            if (result.Successful)
            {
                LoginProgressIndicator.IsRunning = false;
                Navigation.InsertPageBefore(_onlineAPIPage, this);
                await Navigation.PopAsync();
            }
            else
                await DisplayAlert("Error", "Error Logging In", "Dismiss");
        }
    }
}
