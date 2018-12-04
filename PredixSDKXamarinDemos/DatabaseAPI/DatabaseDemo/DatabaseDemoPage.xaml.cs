using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DatabaseDemo
{
    public partial class DatabaseDemoPage : ContentPage
    {
        private ResultsListPage _resultsPage;

        public DatabaseDemoPage()
        {
            InitializeComponent();
            ViewDataButton.Clicked += ViewDataButton_Clicked;
            BindingContext = new MainPageViewModel();
        }

        async void ViewDataButton_Clicked(object sender, System.EventArgs e)
        {
            if (_resultsPage == null)
                _resultsPage = new ResultsListPage();

            var viewModel = (MainPageViewModel)BindingContext;
            var documents = await viewModel.FetchDocumentsAsync();
            _resultsPage.BindingContext = new ResultListViewModel(documents);
            await Navigation.PushAsync(_resultsPage);
        }
    }
}
