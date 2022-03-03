using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace FinProj.Models
{
    public class ApplicationContext : DbContext
    {
        
        public ApplicationContext() : base()
        {

        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Ticker> Tickers { get; set; }        
    }

    public class DbFunc
    {
        public IConfiguration Configuration { get; }

        public static List<Ticker> ReadTickers()
        {
            using (ApplicationContext db = new ApplicationContext(Configuration.GetConnectionString("SqlServer")))
            {
                if((db.Tickers.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                { 
                    return db.Tickers.ToList();
                }
                else
                {
                    WriteTickersToDb();
                    return ParseModule.Parse.GetTickers("https://finance.yahoo.com/commodities");
                }
            } 
        } 

        public static void WriteTickersToDb()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                List<Ticker> tickers = ParseModule.Parse.GetTickers("https://finance.yahoo.com/commodities");
                foreach (var ticker in tickers)
                {                    
                    db.Tickers.Add(ticker);                    
                }
                db.SaveChanges();
            }  
        }           
    }
}