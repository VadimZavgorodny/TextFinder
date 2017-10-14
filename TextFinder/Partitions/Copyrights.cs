using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Copyrights - Копирайт
     *
     */
    class Copyrights : Partition
    {
        public int id = 7;

        protected int minLength = 10;

        protected int maxLength = 512;

        protected bool isSentence = true;

        protected string[] keywords = { "Copyrights", "All Rights Reserved", "Reprints and permissions", "Licensee", "©", "Full terms and conditions of use", "Копирайт" };
    }
}
