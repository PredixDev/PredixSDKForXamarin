using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AuthenticationDemo
{
    public partial class OnlineAPIPage : ContentPage
    {
        public OnlineAPIPage()
        {
            InitializeComponent();
            OnlineAPIButton.Clicked += OnlineAPIButton_Clicked;
            BindingContext = new OnlineAPIPageViewModel();
        }

        async void OnlineAPIButton_Clicked(object sender, EventArgs e)
        {
            StatusIndicator.IsRunning = true;
            var viewModel = (OnlineAPIPageViewModel)BindingContext;
            var response = await viewModel.SendOnlineRequest("https://predixsdkhelloapi.run.aws-usw02-pr.ice.predix.io/hello");
            URLResponseLabel.Text = response.URLResponseData;
            URLStatusLabel.Text = response.URLStatusData;
            StatusIndicator.IsRunning = false;
        }
    }
}
