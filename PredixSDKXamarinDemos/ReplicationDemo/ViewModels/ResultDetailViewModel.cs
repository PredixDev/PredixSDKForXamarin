using System;
using Xamarin.Forms;

namespace ReplicationDemo
{
    public class ResultDetailViewModel : ContentPage
    {
        private readonly FruitDocument _doc;

        public ResultDetailViewModel(FruitDocument doc)
        {
            _doc = doc;
        }

        public string Id => _doc.Id;
        public DateTime CreatedDateTime => _doc.CreateDate;
        public DateTime UpdatedDateTime => _doc.LastChange;
        public string FruitName => _doc.Fruit;

        public string Notes
        {
            get => _doc.Notes;
            set
            {
                _doc.Notes = value;
            }
        }
    }

    // TODO: Work on attachment support

    //public class ResultDetailViewModel : BaseViewModel
    //{
    //    private readonly Document _doc;
    //    private readonly Attachment _attachment;

    //    public ResultDetailViewModel(Document doc, Attachment attachment)
    //    {
    //        _doc = doc;
    //        _attachment = attachment;
    //    }

    //    public string FruitName => _doc.Fruit;
    //    public byte[] imageData => _attachment.imageData;
    //    public string Notes => _doc.Notes;
    //}
}
