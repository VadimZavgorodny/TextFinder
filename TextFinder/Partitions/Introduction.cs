using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Introduction - Введение
     *
     */
    class Introduction : Partition
    {
        public int id = 13;

        protected int minLength = 64;

        protected int maxLength = 4096;

        protected bool isSentence = true;

        protected string[] keywords = { "Introduction", "Background", "Basic definitions", "Введение" };
    }
}
