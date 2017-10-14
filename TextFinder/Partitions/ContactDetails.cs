using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Contact Details - Контактные данные
     *
     */
    class ContactDetails : Partition
    {
        public int id = 6;

        protected int minLength = 10;

        protected int maxLength = 512;

        protected bool isSentence = true;

        protected string[] keywords = { "Contact details", "Author details", "E-mail", "Correspondence", "Tel.", "Fax", "Контактные данные" };
    }
}
