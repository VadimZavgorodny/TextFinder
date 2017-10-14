using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Gratitudes - Благодарности
     *
     */
    class Gratitudes : Partition
    {
        public int id = 27;

        protected int minLength = 32;

        protected int maxLength = 8192;

        protected bool isSentence = true;

        protected string[] keywords = { "Gratitudes", "Acknowledgements", "Grateful to", "Provided by", "Support", "Funding", "Grant", "Help", "Благодарности" };
    }
}
