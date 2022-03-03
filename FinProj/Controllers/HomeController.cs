using FinProj.Models;
using FinProj.ParseModule;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinProj.Controllers
{    
    public class HomeController : Controller
    {
        
        public IActionResult Index(string startDate, string endDate, string ticker = "es=f", string period = "1d")
        {
            QuoteParam param = new QuoteParam
            {
                Ticker = ticker,
                Period = period,
                StartPeriod = DateTime.Now.Subtract(new TimeSpan(50, 0, 0, 0)),
                EndPeriod = DateTime.Now                
            };

            ViewBag.Ticker = ticker;
            ViewData["tickersList"] = DbFunc.ReadTickers();

            return View("Index", CreateRequest(param));
        }

        public List<Quote> CreateRequest(QuoteParam param)
        {
            var url = $"https://finance.yahoo.com/quote/{param.Ticker}/history?period1={(param.StartPeriod.AddDays(1)).ToUnixTimeSeconds()}&period2={param.EndPeriod.ToUnixTimeSeconds()}&interval={param.Period}&filter=history&frequency=1d&includeAdjustedClose=true";

            var quotes = new List<double>();
            var datesHeaders = Parse.GetQuotesHeaders(ref quotes, url);
            List<Quote> Quotes = Parse.ParseQuotes(datesHeaders, quotes);

            return Quotes;
        }
    }
}