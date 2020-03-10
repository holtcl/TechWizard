using System;

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

        public string mostRelevantDate
        {
            get
            {
                if (!(acceptDate == null))
                    return "Accepted " + acceptDate.Value.ToString("MMMM d, yyyy");
                else if (!(completedDate == null))
                    return "Completed " + completedDate.Value.ToString("MMMM d, yyyy");
                else
                    return "Opened " + openDate.ToString("MMMM d, yyyy");

            }
        }

        public string status
        {
            get
            {
                if (!(acceptDate == null))
                    return "In progress";
                else if (!(completedDate == null))
                    return "Complete";
                else
                    return "Awaiting Wizard";
            }
        }

        public string statusColor
        {
            get
            {
                if (!(acceptDate == null))
                    return "#dec102"; // Yellow
                else if (!(completedDate == null))
                    return "#15b03e"; // Green
                else
                    return "#de0202"; // Red
            }
        }

        public String formattedPrice
        {
            get
            {
                String returnPrice = priceInCents + "";
                return "$" + returnPrice.Insert(returnPrice.Length - 2, ".");
            }
        }
    }
}