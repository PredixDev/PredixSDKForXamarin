using Xamarin.Forms;

namespace DatabaseDemo
{
    public partial class ResultDetailPage : ContentPage
    {
        public ResultDetailPage()
        {
            InitializeComponent();

            ID.SetBinding(Label.TextProperty, nameof(ResultDetailViewModel.Id));
            CreatedDateTimeLabel.SetBinding(Label.FormattedTextProperty, nameof(ResultDetailViewModel.CreatedDateTime), converter: FormattedDateTimeConverter.Instance);
            UpdatedDateTimeLabel.SetBinding(Label.FormattedTextProperty, nameof(ResultDetailViewModel.UpdatedDateTime), converter: FormattedDateTimeConverter.Instance);
            FruitName.SetBinding(Label.TextProperty, nameof(ResultDetailViewModel.FruitName));
            Notes.SetBinding(Entry.TextProperty, nameof(ResultDetailViewModel.Notes));
        }
    }
}
