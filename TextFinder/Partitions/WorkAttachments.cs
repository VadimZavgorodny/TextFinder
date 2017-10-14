using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Work attachments - Приложения к работе
     *
     */
    class WorkAttachments : Partition
    {
        public int id = 30;

        protected int minLength = 32;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Work attachments", "Appendix", "Appendices", "Additional", "Приложения к работе" };
    }
}
