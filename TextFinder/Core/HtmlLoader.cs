using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace TextFinder.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string[] urls; 

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            urls = settings.BaseUrls;       
        }      

        public async Task<List<Documet>> GetSource()
        {
            List<Documet> dinosaurs = new List<Documet>();
            

            foreach (var item in urls)
            {
                var document = new Documet();
                var response = await client.GetAsync(item);

                document.Url = item;
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    document.Source = await response.Content.ReadAsStringAsync();
                    dinosaurs.Add(document);
                }
            }

            return dinosaurs;
        }
    }
}
