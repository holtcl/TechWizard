using System;
using System.Collections.Generic;
using System.Text;

namespace TechWizard
{
    public class RequestForDisplay
    {
        public long requestID { get; set; }
        public string description { get; set; }
        public string user { get; set; }
        public string skill { get; set; }
        public string wizard { get; set; }
        public DateTime openDate { get; set; }
        public DateTime? acceptDate { get; set; }
        public DateTime? completedDate { get; set; }
        public int priceInCents { get; set; }
        public string title { get; set; }
        public string contactMethod { get; set; }
    }
}