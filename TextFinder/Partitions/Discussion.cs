using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Discussion - Обсуждение
     *
     */
    class Discussion : Partition
    {
        public int id = 22;

        protected int minLength = 32;

        protected int maxLength = 4096;

        protected bool isSentence = true;

        protected string[] keywords = { "Discussion", "Results and discussion", "Extensions and applications", "Applications and commercialization", "Обсуждение" };
    }
}
