using System;
using Toolbox.Portable;

namespace IndexAndQueryDemo
{
    public class QueryDetailViewModel : BaseViewModel
    {
        private readonly WordItem _word;

        public QueryDetailViewModel(WordItem word)
        {
            _word = word;
        }

        public string Word => _word.Word;
        public string Definition => _word.Definition;
    }
}
