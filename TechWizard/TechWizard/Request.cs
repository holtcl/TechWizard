using System;
using System.Collections.Generic;
using System.Text;

namespace TechWizard
{
    class Request
    {
        public long requestID { get; set; }
        public string description { get; set; }
        public int user { get; set; }
        public int wizard { get; set; }
        public DateTime openDate { get; set; }
        public DateTime acceptDate { get; set; }
        public DateTime completedDate { get; set; }
        public string supportType { get; set; }
        public int status { get; set; }



    }
}
