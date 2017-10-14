using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Conventions - Условные обозначения
     *
     */
    class Conventions : Partition
    {
        public int id = 12;

        protected int minLength = 10;

        protected int maxLength = 512;

        protected bool isSentence = true;

        protected string[] keywords = { "Conventions", "Nomenclature", "Abbreviations", "Условные обозначения" };
    }
}
