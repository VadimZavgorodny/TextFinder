using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Core
{
    interface IParserSettings
    {
        string[] BaseUrls { get; set; }      
    }
}
