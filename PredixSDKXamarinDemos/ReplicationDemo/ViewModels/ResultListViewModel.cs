using System;
using System.Collections.ObjectModel;
//using Toolbox.Portable;

namespace ReplicationDemo
{
    public class ResultListViewModel
    {
        ObservableCollection<FruitDocument> _fetchedResultsData = new ObservableCollection<FruitDocument>();
        //ObservableCollection<Attachment> _fetchedAttachmentData = new ObservableCollection<Attachment>();

        public ObservableCollection<FruitDocument> FetchedResultsData
        {
            get { return _fetchedResultsData; }
            set { _fetchedResultsData = value; }

            // // Previous Block
            // get { return _fetchedResultsData; }
            // set { RaiseAndUpdate(ref _fetchedResultsData, value); }
        }

        //public ObservableCollection<Attachment> FetchedAttachmentData
        //{
        //    get { return _fetchedAttachmentData; }
        //    set { RaiseAndUpdate(ref _fetchedAttachmentData, value); }
        //}

        //public ResultListViewModel(ObservableCollection<ReplicationDataItem> resultsList)
        //{
        //    foreach (var item in resultsList)
        //    {
        //        _fetchedResultsData.Add(item.Document);
        //        //_fetchedAttachmentData.Add(item.Attachment);
        //    }
        //}

        public ResultListViewModel(ObservableCollection<FruitDocument> resultsList)
        {
            _fetchedResultsData = resultsList;
        }
    }


}
