using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Problem statement - Постановка задачи 
     *
     */
    class ProblemStatement : Partition
    {
        public int id = 17;

        protected int minLength = 64;

        protected int maxLength = 4096;

        protected bool isSentence = true;

        protected string[] keywords = { "Problem statement", "Implemented modifications", "Motivation and assumptions", "Assumptions", "Algorithm description", "Постановка задачи" };
    }
}
