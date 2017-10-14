using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Tables - Таблицы
     *
     */
    class Tables : Partition
    {
        public int id = 20;

        protected int minLength = 10;

        protected int maxLength = 512;

        protected bool isSentence = false;

        protected string[] keywords = { "Tables", "Table", "Selected data sets", "Overview", "Comparison", "Таблицы" };
    }
}
