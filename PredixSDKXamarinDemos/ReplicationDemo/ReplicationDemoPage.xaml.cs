using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ReplicationDemo
{
    public partial class ReplicationDemoPage : ContentPage
    {
        private ResultsListPage _resultsListPage;

        public ReplicationDemoPage()
        {
            InitializeComponent();
            LoginButton.Clicked += LoginButton_Clicked;
            ViewDataButton.Clicked += ViewDataButton_Clicked;
            BindingContext = new MainPageViewModel(StatusLabel, ViewDataButton);
        }

        async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            LoginProgressIndicator.IsRunning = true;

            var viewModel = (MainPageViewModel)BindingContext;
            var result = await viewModel.PerformLoginAsync(UsernameField.Text, PasswordField.Text);
            if (result.Successful)
            {
                LoginProgressIndicator.IsRunning = false;
            }
            else
                await DisplayAlert("Error", "Error Logging In", "Dismiss");
        }

        async void ViewDataButton_Clicked(object sender, System.EventArgs e)
        {
            if (_resultsListPage == null)
                _resultsListPage = new ResultsListPage();

            var viewModel = (MainPageViewModel)BindingContext;
            var documents = viewModel.FetchDocuments();
            _resultsListPage.BindingContext = new ResultListViewModel(documents);
            await Navigation.PushAsync(_resultsListPage);
        }
    }
}
