using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Academic Editor - Академический редактор
     *
     */
    class AcademicEditor : Partition
    {
        public int id = 9;

        protected int minLength = 10;

        protected int maxLength = 512;

        protected bool isSentence = true;

        protected string[] keywords = { "Academic editor", "Technical editor", "Академический редактор" };
    }
}
