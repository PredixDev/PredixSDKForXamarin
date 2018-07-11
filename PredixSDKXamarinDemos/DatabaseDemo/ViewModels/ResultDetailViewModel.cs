using System;

using Xamarin.Forms;

namespace DatabaseDemo
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

                // // Previous Block
                // _doc.Notes = value;
                // Raise();
            }
        }
    }
}

