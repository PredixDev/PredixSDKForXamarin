using System.Collections.ObjectModel;

namespace DatabaseDemo
{
    public class ResultListViewModel
    {
        ObservableCollection<FruitDocument> _fetchedResultsData = new ObservableCollection<FruitDocument>();
        public ObservableCollection<FruitDocument> FetchedResultsData
        {
            get { return _fetchedResultsData; }
            set { _fetchedResultsData = value; }

            // // Previous Block
            // get { return _fetchedResultsData; }
            // set { RaiseAndUpdate(ref _fetchedResultsData, value); }
        }

        public ResultListViewModel(ObservableCollection<FruitDocument> resultsList)
        {
            _fetchedResultsData = resultsList;
        }
    }
}