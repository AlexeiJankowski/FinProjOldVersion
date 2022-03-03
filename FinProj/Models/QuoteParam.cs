using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinProj.Models
{
    public class QuoteParam
    {
        public string Ticker { get; set; }
        public string Period { get; set; }
        public DateTimeOffset StartPeriod { get; set; }
        public DateTimeOffset EndPeriod { get; set; }
    }
}
