using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Summary - Выводы
     *
     */
    class Summary : Partition
    {
        public int id = 25;

        protected int minLength = 32;

        protected int maxLength = 4096;

        protected bool isSentence = true;

        protected string[] keywords = { "Summary", "Conclusion", "Выводы" };
    }
}
