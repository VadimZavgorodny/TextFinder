namespace TextFinder.Core.hindawi
{
    class HindawiSettings : IParserSettings
    {
        private string[] baseUrls;

        public string[] BaseUrls
        {
            get
            {
                return baseUrls;
            }
            set
            {
                baseUrls = value;
            }
        }
    }
}
