using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Journal - Журнал
     *
     */

    class Journal : Partition
    {
        public int id = 4;

        protected int minLength = 5;

        protected int maxLength = 256;

        protected bool isSentence = true;

        protected string[] keywords = { "Journal", "Journal title", "Vol.", "Issue", "ISSN", "Журнал" };
    }
}
