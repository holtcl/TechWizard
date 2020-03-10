using System;

namespace TechWizard.Models
{
    public class User
    {

        public long? ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Boolean isWizard { get; set; }
        //public ??? pic {get; set;}
    }
}
