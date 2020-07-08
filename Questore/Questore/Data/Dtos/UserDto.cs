using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questore.Data.Dtos
{
    public abstract class UserDto
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Role { get; protected set; }
    }
}
