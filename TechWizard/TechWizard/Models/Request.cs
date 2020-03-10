using System;

namespace TechWizard.Models
{
    public class Request
    {
        public long requestID { get; set; }
        public string description { get; set; }
        public int user { get; set; }
        public int skill { get; set; }
        public int? wizard { get; set; }
        public DateTime openDate { get; set; }
        public DateTime? acceptDate { get; set; }
        public DateTime? completedDate { get; set; }
        public int priceInCents { get; set; }
        public string title { get; set; }
        public int contactMethod { get; set; }
    }
}