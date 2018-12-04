using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace IndexAndQueryDemo
{
    public partial class QueryListPage : ContentPage
    {
        private QueryDetailPage _detailPage;
        private QueryListPageViewModel ViewModel => BindingContext as QueryListPageViewModel;

        public QueryListPage()
        {
            InitializeComponent();
            BindingContext = new QueryListPageViewModel();

            QueryResultsList.SetBinding(ItemsView<Cell>.ItemsSourceProperty, nameof(QueryListPageViewModel.QueryResultsData));
            QueryResultsList.ItemTemplate = new DataTemplate(() =>
            {
                var textViewCell = new TextCell();
                textViewCell.SetBinding(TextCell.TextProperty, nameof(WordItem.Word));
                textViewCell.SetBinding(TextCell.DetailProperty, nameof(WordItem.Definition));
                return textViewCell;
            });

            QueryResultsList.ItemSelected += QueryResultsList_ItemSelected;
            QueryResultsSearchBar.TextChanged += QueryResultsSearchBar_TextChangedAsync;
            CreateSampleDatabase();
        }

        public void CreateSampleDatabase()
        {
            var assembly = typeof(QueryListPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("IndexAndQueryDemo.dictionary.json");

            ViewModel.CreateSampleDocumentsIfNeededAsync(stream).ConfigureAwait(false);
        }

        void QueryResultsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (_detailPage == null)
                _detailPage = new QueryDetailPage();

            _detailPage.BindingContext = new QueryDetailViewModel(e.SelectedItem as WordItem);
            Navigation.PushAsync(_detailPage);
        }

        async void QueryResultsSearchBar_TextChangedAsync(object sender, TextChangedEventArgs e)
        {
            await ViewModel.QueryDatabaseAsync(e.NewTextValue.ToLower());
        }
    }
}