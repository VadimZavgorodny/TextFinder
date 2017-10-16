using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder.Core
{
    class ParserWorker 
    {
        IParser parser;
        IParserSettings parserSettings;

        HtmlLoader loader;

        bool isActive;

        #region Properties

        public IParser Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
            }
        }

        public IParserSettings Settings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }

        #endregion

        public event Action<object, Documet> OnNewData;
        public event Action<object> OnCompletedParsed;

        public ParserWorker(IParser parser)
        {
            this.parser = parser;
        }

        public ParserWorker(IParser parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }

        public void Start()
        {
            isActive = true;
            Worker();
        }

        public void Abort()
        {
            isActive = false;
        }

        private async void Worker()
        {            
            var source = await loader.GetSource();
            var domParser = new HtmlParser();

            foreach (var item in source)
            {
                var document = await domParser.ParseAsync(item.Source);
                item.Parsed = parser.Parse(document);
                
                OnNewData?.Invoke(this, item); 
            }
           
            OnCompletedParsed?.Invoke(this);
        }
    }
}
