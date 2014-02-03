using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleCount.Models
{
    public class CalcParameter
    {
        private decimal _a;
        private decimal _b;

        public CalcParameter(decimal a, decimal b)
        {
            _a = a;
            _b = b;
        }

        public decimal A
        {
            get { return _a; }
            set { _a = value; }
        }

        public decimal B
        {
            get { return _b; }
            set { _b = value; }
        }
    }
}
