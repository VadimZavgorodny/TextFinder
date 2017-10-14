using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Results - Результаты 
     *
     */
    class Results : Partition
    {
        public int id = 18;

        protected int minLength = 64;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Results", "Research results", "Practical results", "Case studies", "Applications", "Scale up and design", "Результаты" };
    }
}
