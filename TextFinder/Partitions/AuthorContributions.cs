using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Author contributions - Авторский вклад в работу
     *
     */
    class AuthorContributions : Partition
    {
        public int id = 28;

        protected int minLength = 32;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Author contributions", "Авторский вклад" };
    }
}
