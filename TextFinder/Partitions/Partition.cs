using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextFinder.Partitions
{
    public class Partition
    {
        // ID of the Partition
        public int id;
        
        // Stores found text of the Partition
        protected string text = "";
        
        // Defines minimal length of the Partition
        protected int minLength = 0;
        
        // Defines maximal length of the Partition
        protected int maxLength = 0;
        
        // Defines is Partition a sentence
        protected bool isSentence;
        
        // Stores current status of finding Partition
        protected bool isFound = false;
        
        // Stores keywords for finding Partition in text
        protected string[] keywords;

        // Checks, has found by findRawTextByKeywords text allowable min and max length or not
        protected bool checkLength()
        {
            return (this.text.Length >= this.minLength && this.text.Length <= this.maxLength);
        }

        // Finds Partition text using keywords via regular expressions
        protected virtual string findRawTextByKeywords(string articleText)
        {
            string resultString = String.Empty;
            if (this.keywords.Length > 0 && this.keywords != null)
            {
                var regexString = String.Join("|", this.keywords);
                var regex = new Regex(string.Format("[^.!?;]*({0})[^.?!;]*[.?!;]", regexString));
                var results = regex.Matches(articleText);

                for (int i = 0; i < results.Count; i++)
                {
                    resultString += results[i].Value.Trim();
                }
            }

            this.text = resultString;
            return this.text;
        }

        // Main function to find Partition. Calls the others and returning Partition text as result.
        public string find(string articleText)
        {
            string rawTextByKeywords = findRawTextByKeywords(articleText);

            this.isFound = checkLength();

            return this.text;
        }
        
    }
}
