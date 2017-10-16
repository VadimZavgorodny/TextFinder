using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Core
{
    class Documet
    {
        public string source;

        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

        public string url;

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        public string parsed;

        public string Parsed
        {
            get
            {
                return parsed;
            }
            set
            {
                parsed = value;
            }
        }

    }
}
