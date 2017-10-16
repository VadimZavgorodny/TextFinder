using AngleSharp.Dom.Html;

namespace TextFinder.Core
{
    interface IParser
    {
        string Parse(IHtmlDocument document);        
    }
}
