
using Xamarin.Forms;

namespace ReplicationDemo
{
    public partial class ReplicationDemoPage : ContentPage
    {
        private ResultsListPage _resultsPage;

        public ReplicationDemoPage()
        {
            InitializeComponent();
            LoginButton.Clicked += LoginButton_Clicked;
            ViewDataButton.Clicked += ViewDataButton_Clicked;
            BindingContext = new MainPageViewModel();
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
            if (_resultsPage == null)
                _resultsPage = new ResultsListPage();

            var viewModel = (MainPageViewModel)BindingContext;
            var documentResults = await viewModel.FetchDocumentsAsync();
            _resultsPage.BindingContext = new ResultListViewModel(documentResults);
            await Navigation.PushAsync(_resultsPage);
        }
    }
}
