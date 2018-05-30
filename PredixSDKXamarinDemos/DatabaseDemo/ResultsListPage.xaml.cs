using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DatabaseDemo
{
    public partial class ResultsListPage : ContentPage
    {
        private ResultDetailPage _detailPage;

        public ResultsListPage()
        {
            InitializeComponent();

            ResultsList.SetBinding(ItemsView<Cell>.ItemsSourceProperty, nameof(ResultListViewModel.FetchedResultsData));
            ResultsList.ItemTemplate = new DataTemplate(() =>
            {
                var textViewCell = new TextCell();
                textViewCell.SetBinding(TextCell.TextProperty, nameof(FruitDocument.Fruit));
                return textViewCell;
            });

            ResultsList.ItemSelected += ItemSelected;
        }

        private ResultListViewModel ViewModel => BindingContext as ResultListViewModel;

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (_detailPage == null)
                _detailPage = new ResultDetailPage();

            _detailPage.BindingContext = new ResultDetailViewModel(e.SelectedItem as FruitDocument);
            Navigation.PushAsync(_detailPage);
        }
    }
}
