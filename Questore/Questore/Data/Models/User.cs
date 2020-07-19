using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questore.Data.Models
{
    public abstract class User
    {
        public int CredentialsId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; protected set; }
    }
}
