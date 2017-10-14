using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Publisher - Издатель
     *
     */
    class Publisher : Partition
    {
        public int id = 8;

        protected int minLength = 5;

        protected int maxLength = 256;

        protected bool isSentence = true;

        protected string[] keywords = { "Publisher", "Published by", "Printed in", "Production and hosting by", "Издатель" };
    }
}
