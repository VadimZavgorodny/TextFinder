using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Annotation - Аннотация
     *
     */
    class Annotation : Partition
    {
        public int id = 10;

        protected int minLength = 32;

        protected int maxLength = 1024;

        protected bool isSentence = true;

        protected string[] keywords = { "Annotation", "Abstract", "Аннотация" };
    }
}
