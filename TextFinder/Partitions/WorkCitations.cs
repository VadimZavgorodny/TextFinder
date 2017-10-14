using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Work citations - Цитирование работы
     *
     */
    class WorkCitations : Partition
    {
        public int id = 29;

        protected int minLength = 32;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Work citations", "Cite this article", "Cite article", "Article cite", "Цитирование работы" };
    }
}
