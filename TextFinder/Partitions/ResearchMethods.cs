using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Research methods - Методы исследования 
     *
     */
    class ResearchMethods : Partition
    {
        public int id = 16;

        protected int minLength = 64;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Research methods", "Materials", "Methods", "Research technique", "Solution methodology", "Методы исследования" };
    }
}
