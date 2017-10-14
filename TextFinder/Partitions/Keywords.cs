using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Keywords - Ключевые слова
     *
     */
    class Keywords : Partition
    {
        public int id = 11;

        protected int minLength = 10;

        protected int maxLength = 512;

        protected bool isSentence = false;

        protected string[] keywords = { "Keywords", "Ключевые слова" };
    }
}
