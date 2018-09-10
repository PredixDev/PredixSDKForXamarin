using System;
using System.Collections.ObjectModel;
//using Toolbox.Portable;

namespace ReplicationDemo
{
    public class ResultListViewModel
    {
        ObservableCollection<FruitDocument> _fetchedResultsData = new ObservableCollection<FruitDocument>();
        ObservableCollection<FruitAttachment> _fetchedAttachmentData = new ObservableCollection<FruitAttachment>();

        public ObservableCollection<FruitDocument> FetchedResultsData
        {
            get { return _fetchedResultsData; }
            set { _fetchedResultsData = value; }
        }

        public ObservableCollection<FruitAttachment> FetchedAttachmentData
        {
            get { return _fetchedAttachmentData; }
            set { _fetchedAttachmentData = value; }
        }

        public ResultListViewModel(ObservableCollection<ReplicationDataItem> resultsList)
        {
            foreach (var item in resultsList)
            {
                _fetchedResultsData.Add(item.Document);
                _fetchedAttachmentData.Add(item.Attachment);
            }
        }
    }
}
