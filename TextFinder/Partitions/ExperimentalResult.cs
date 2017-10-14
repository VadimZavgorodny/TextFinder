using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Experimental Result - Экспериментальные результаты
     *
     */
    class ExperimentalResult : Partition
    {
        public int id = 22;

        protected int minLength = 32;

        protected int maxLength = 4096;

        protected bool isSentence = true;

        protected string[] keywords = { "Experimental result", "Experimental research", "Design of experiment", "Physical modelling", "Processing", "Performance", "Mechanical properties", "Экспериментальные результаты" };
    }
}
