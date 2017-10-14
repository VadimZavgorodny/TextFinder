using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* References - Список литературы
     *
     */
    class References : Partition
    {
        public int id = 26;

        protected int minLength = 32;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "References", "Literature", "Список литературы" };
    }
}
