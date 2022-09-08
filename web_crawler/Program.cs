using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace web_crawler
{
    internal class Program
    {
        static void Main(string[] args)
        {

            StartCrawlerAsync();
            Console.ReadLine();

        }

        private static async void StartCrawlerAsync()
        {
            var url = "http://www.automobile.tn/neuf/bmw.3/";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("versions-item")).ToList();

            var cars = new List<Car>();

            foreach (var div in divs)
            {
                var car = new Car()
                {
                    Model = div?.Descendants("h2")?.FirstOrDefault()?.InnerText,
                    Price = div?.Descendants("div")?.FirstOrDefault()?.InnerText,
                    ImageUrl = div?.Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value,
                    Link = div?.Descendants("a")?.FirstOrDefault()?.ChildAttributes("href")?.FirstOrDefault()?.Value
                };

                cars.Add(car);
            }
        }

        [DebuggerDisplay("{Model},{Price}")]
        public class Car
        {
            public string Model { get; set; }
            public string Price { get; set; }
            public string Link { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
