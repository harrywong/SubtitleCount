using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleCount
{
    public interface ISubtitleCount
    {
        CountResult Count(string content);
    }
}
