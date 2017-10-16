using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using System.Text.RegularExpressions;

namespace TextFinder.Core.hindawi
{
    class HindawiParser : IParser
    {
        public string Parse(IHtmlDocument document)
        {
            var list = new List<string>();
            var items = document.QuerySelectorAll(".middle_content");

            foreach (var item in items)
            {
                list.Add(Regex.Replace(item.TextContent.Trim(), @"\s+", " "));
            }

            return String.Join("", list.ToArray());
        }

    }

}
