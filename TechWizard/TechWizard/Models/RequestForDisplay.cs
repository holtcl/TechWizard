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
        public int? hours { get; set; }
        public DateTime openDate { get; set; }
        public DateTime? acceptDate { get; set; }
        public DateTime? completedDate { get; set; }
        public int priceInCents { get; set; }
        public string title { get; set; }
        public string contactMethod { get; set; }

        public string totalCost 
        {
            get {
                int totalPrice = priceInCents * hours.Value;

                if (priceInCents < 10)
                    return "$0.0" + totalPrice;
                else if (priceInCents < 100)
                    return "$0." + totalPrice;
                String returnPrice = totalPrice + "";
                return "$" + returnPrice.Insert(returnPrice.Length - 2, ".");
            }
        }

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
                if (!(completedDate == null))
                    return "Complete";
                else if (!(acceptDate == null))
                    if (hours == null)
                        return "In progress";
                    else
                        return "Awaiting Client Approval";
                else
                    return "Awaiting Wizard";
            }
        }

        public string statusColor
        {
            get
            {
                if (!(completedDate == null))
                    return "#15b03e"; // Green
                else if (!(acceptDate == null))
                    return "#dec102"; // Yellow
                else
                    return "#de0202"; // Red
            }
        }

        public String formattedPrice
        {
            get
            {
                if (priceInCents < 10)
                    return "$0.0" + priceInCents;
                else if (priceInCents < 100)
                    return "$0." + priceInCents;
                String returnPrice = priceInCents + "";
                return "$" + returnPrice.Insert(returnPrice.Length - 2, ".");
            }
        }
    }
}