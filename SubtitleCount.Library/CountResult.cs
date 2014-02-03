using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleCount
{
    public class CountResult
    {
        private int _words;
        private int _lines;
        private List<int> _lineWords; 

        public CountResult(int words, int lines)
        {
            _words = words;
            _lines = lines;
        }

        public int Words
        {
            get { return _words; }
            set { _words = value; }
        }

        public int Lines
        {
            get { return _lines; }
            set { _lines = value; }
        }

        public List<int> LineWords
        {
            get { return _lineWords; }
            set { _lineWords = value; }
        }
    }
}
