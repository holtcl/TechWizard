using System;

namespace TechWizard
{
    class Invoice
    {
        public long invoiceID { get; set; }
        public int user { get; set; }
        public int wizard { get; set; }
        public double hoursWorked { get; set; }
        public double rate { get; set; }
        public double amountDue { get; set; }
        public DateTime openDate { get; set; }
        public DateTime dueDate { get; set; }
        public int status { get; set; }

    }
}
