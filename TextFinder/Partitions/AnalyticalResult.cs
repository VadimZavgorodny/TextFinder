using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Analytical Result - Аналитические теоретические результаты
     *
     */
    class AnalyticalResult : Partition
    {
        public int id = 21;

        protected int minLength = 32;

        protected int maxLength = 4096;

        protected bool isSentence = true;

        protected string[] keywords = { "Analytical result", "Analytical", "Mathematical modeling", "Theoretical", "Mathematical framework", "Аналитические теоретические результаты", "Аналитические результаты" };
    }
}
