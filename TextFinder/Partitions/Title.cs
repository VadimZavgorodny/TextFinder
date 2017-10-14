using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Article Title - Заголовок статьи
     *
     */

    class Title : Partition
    {
        public int id = 3;

        protected int minLength = 5;

        protected int maxLength = 256;

        protected bool isSentence = true;

        protected string[] keywords = { "Title", "Artical title", "Заголовок" };
    }
}
