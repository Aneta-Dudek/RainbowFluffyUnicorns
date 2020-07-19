using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questore.Data.Models
{
    public class Admin : User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string PhotoId { get; set; }
        public Admin()
        {
            Role = "admin";
        }
    }
}
