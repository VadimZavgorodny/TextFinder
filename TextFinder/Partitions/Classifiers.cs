using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* Article Classifier - Классификатор статьи
     * Examples how can it looks like: "004.8", "004.891.3"
     * Structure and features:
     * - begins and ends with digits
     * - has one or few dots in the middle
     * - contains only with digits and dots
     * - basic structure is "DIGITS DOT DIGITS [DOT DIGITS]"
     */

    class Classifiers : Partition
    {
        public int id = 2;

        protected int minLength = 3;

        protected int maxLength = 20;

        protected bool isSentence = false;

        protected string[] keywords = { "Classifier", "UDC", "PACS", "MSC", "Классификатор" };
    }
}
