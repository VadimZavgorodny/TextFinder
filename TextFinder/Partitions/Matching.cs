using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Matching - Сопоставление
     *
     */
    class Matching : Partition
    {
        public int id = 23;

        protected int minLength = 32;

        protected int maxLength = 2048;

        protected bool isSentence = true;

        protected string[] keywords = { "Matching", "Evaluation", "Сопоставление" };
    }
}
