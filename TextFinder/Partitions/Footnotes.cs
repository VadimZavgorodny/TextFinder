using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Footnotes - Примечания
     *
     */
    class Footnotes : Partition
    {
        public int id = 31;

        protected int minLength = 32;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Footnotes", "Примечания" };
    }
}
