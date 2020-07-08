using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questore.Data.Dtos
{
    public class AdminDto : UserDto
    {
        public AdminDto()
        {
            Role = "admin";
        }
    }
}
