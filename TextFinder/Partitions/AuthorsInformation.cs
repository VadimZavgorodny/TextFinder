using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Authors information - Дополнительные сведения об авторах
     *
     */
    class AuthorsInformation : Partition
    {
        public int id = 28;

        protected int minLength = 32;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Authors information", "Сведения об авторах" };
    }
}
