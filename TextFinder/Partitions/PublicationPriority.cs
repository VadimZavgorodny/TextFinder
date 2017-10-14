using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Publication Priority - Приоритет публикации
     *
     */
    class PublicationPriority : Partition
    {
        public int id = 5;

        protected int minLength = 5;

        protected int maxLength = 256;

        protected bool isSentence = true;

        protected string[] keywords = { "Publication priority", "Article History", "Received", "Revised", "Final form", "Accepted", "Published", "Article published online", "Приоритет публикации" };
    }
}
