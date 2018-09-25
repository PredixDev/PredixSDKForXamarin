using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace IndexAndQueryDemo
{
    public partial class QueryDetailPage : ContentPage
    {
        public QueryDetailPage()
        {
            InitializeComponent();
            WordNameLabel.SetBinding(Label.TextProperty, nameof(QueryDetailViewModel.Word));
            WordDefinitionLabel.SetBinding(Label.TextProperty, nameof(QueryDetailViewModel.Definition));
        }
    }
}
