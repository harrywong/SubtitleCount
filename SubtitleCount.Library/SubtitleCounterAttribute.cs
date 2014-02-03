using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleCount
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SubtitleCounterAttribute : Attribute
    {
        private string _format;

        public SubtitleCounterAttribute(string format)
        {
            _format = format;
        }

        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }
    }
}
