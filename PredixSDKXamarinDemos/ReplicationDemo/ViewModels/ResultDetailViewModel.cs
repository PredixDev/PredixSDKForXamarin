using System;
using Toolbox.Portable;

namespace ReplicationDemo
{
    public class ResultDetailViewModel : BaseViewModel
    {
        private readonly FruitDocument _doc;
        private readonly FruitAttachment _attachment;

        public ResultDetailViewModel(FruitDocument doc, FruitAttachment attachment)
        {
            _doc = doc;
            _attachment = attachment;
        }

        public string Id => _doc.Id;
        public DateTime CreatedDateTime => _doc.CreateDate;
        public DateTime UpdatedDateTime => _doc.LastChange;

        public string FruitName => _doc.Fruit;
        public byte[] imageData => _attachment.imageData;
        public string Notes => _doc.Notes;
    }
}
