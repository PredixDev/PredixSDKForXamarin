using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace ReplicationDemo
{
    public partial class ResultDetailPage : ContentPage
    {
        private ResultDetailViewModel ViewModel => BindingContext as ResultDetailViewModel;

        public ResultDetailPage()
        {
            InitializeComponent();
            FruitName.SetBinding(Label.TextProperty, nameof(ResultDetailViewModel.FruitName));
            FruitImage.Source = ImageSource.FromStream(() => new MemoryStream(ViewModel.imageData));
            Notes.SetBinding(Label.TextProperty, nameof(ResultDetailViewModel.Notes));
        }
    }
}
