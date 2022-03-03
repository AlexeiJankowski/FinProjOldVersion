using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System;
using FinProj.Models;
using System.Linq;

namespace FinProj.ParseModule
{
    public class Parse
    {
        public static HtmlNodeCollection GetQuotesHeaders(ref List<double> quotes, string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            var datesHeaders = doc.DocumentNode.SelectNodes("//td[@class='Py(10px) Ta(start) Pend(10px)']");
            var quoteHeaders = doc.DocumentNode.SelectNodes("//td[@class='Py(10px) Pstart(10px)']");
                     

            foreach(var item in quoteHeaders)
            {
                double temp;
                if (Double.TryParse(item.InnerText.Replace(",", String.Empty).Replace(".", ","), out temp))
                    quotes.Add(temp);
                else
                    quotes.Add(0.0);
            }  
            return datesHeaders;
        }

        public static List<Ticker> GetTickers(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            List<Ticker> result = new List<Ticker>();

            var tickersHeaders = doc.DocumentNode.SelectNodes("//a[@class='Fw(600) C($linkColor)']");
            var tickersNames = doc.DocumentNode.SelectNodes("//a[@class='Fw(600) C($linkColor)']");

            for (int i = 0; i < tickersHeaders.Count; i++)
            {
                if(tickersNames[i].Attributes["title"].Value.Contains("&"))
                {
                    result.Add(new Ticker
                    {
                        TickerValue = tickersHeaders[i].InnerText.ToString(),
                        TickerName = tickersNames[i].Attributes["title"].Value.ToString().Replace("amp;", "")
                    });
                }
                else
                {
                    result.Add(new Ticker
                    {
                        TickerValue = tickersHeaders[i].InnerText.ToString(),
                        TickerName = tickersNames[i].Attributes["title"].Value
                    });
                }                
            }

            return result;
        }

        public static List<Quote> ParseQuotes(HtmlNodeCollection datesHeaders, List<double> quotes)
        {
            List<Quote> Quotes = new List<Quote>();

            foreach (var item in datesHeaders)
            {
                {
                    Quotes.Add(new Quote
                    {
                        Date = DateTime.Parse(item.InnerText),
                        Open = quotes[0],
                        High = quotes[1],
                        Low = quotes[2],
                        Close = quotes[3],
                        Volume = quotes[5]
                    });
                }                
                quotes.RemoveRange(0, 6);
            }
            return Quotes;
        }
    }
}