using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Literature Review - Литературный обзор
     *
     */
    class LiteratureReview : Partition
    {
        public int id = 14;

        protected int minLength = 64;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Literature review", "Existing approaches and factors", "A current state-of-the-art", "Some comments", "Some characteristics", "Литературный обзор" };
    }
}
