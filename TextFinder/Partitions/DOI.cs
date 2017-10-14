using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    /* DOI (Digital Object Identifier) - Цифровой идентификатор объекта
     * Examples how can it looks like: "DOI 10.1007/b136753", "DOI 10.1007/978-3-540-46129-6"
     * Structure:
     * - begins with "DOI" or "doi"
     * - may be a whitespace after it
     * - few digits after it
     * - dot "."
     * - digits, slash and symbols
     */

    class DOI : Partition
    {
        public int id = 1;

        protected int minLength = 10;

        protected int maxLength = 50;

        protected bool isSentence = false;

        protected string[] keywords = { "DOI", "doi" };

        // Overriding parent`s method because this Partition represents a word
        protected override string findRawTextByKeywords(string articleText)
        {
            var regexString = String.Join("|", this.keywords);
            var regex = new Regex(string.Format(@"(?<!\w){0}[\f\t\v]", regexString));
            var results = regex.Matches(articleText);

            this.text = results.Count > 0 ? results[0].Value.Trim() : ""; 
            return this.text;
        }

    }
}
