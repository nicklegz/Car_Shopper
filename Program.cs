using CsvHelper;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System;

namespace Autotrader_Web_Scraper
{
    class Programgit 
    {
        //https://www.autotrader.ca/cars/nissan/370z/?rcp=100&pRng=%2C30000
        static void Main(string[] args)
        {
            WebClient client = new WebClient(); 
            HtmlDocument doc = new HtmlDocument();
            doc.Load(client.OpenRead("https://www.autotrader.ca/cars/nissan/370z/?rcp=100&pRng=%2C30000&trans=Manual"), Encoding.UTF8);

            var prices = doc.DocumentNode.SelectNodes("//span[@class='price-amount']");
            var kms = doc.DocumentNode.SelectNodes("//div[@class='kms']");
            var listingDetails = doc.DocumentNode.SelectNodes("//a[@class='result-title click']");

            var cars = new List<Car>();

            for(int i = 0; i < prices.Count; i++)
            {
                if(i < prices.Count && i < kms.Count && i < listingDetails.Count)
                {
                    cars.Add(new Car
                    {
                        Price = prices[i].InnerText.Trim(), 
                        Mileage = kms[i].InnerText.Trim().Replace("Mileage ", ""), 
                        Details =  listingDetails[i].SelectSingleNode("./span").InnerText.Trim()
                    });
                }
            }

            using (var writer = new StreamWriter(@"S:\Programming\Projects\Autotrader_Web_Scraper\Outputs\example.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(cars);
            }

        }
    }

    class Car
    {
        public string Price { get; set; }
        public string Mileage { get; set; }
        public string Details { get; set; }

    }
}
