using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Questore.Data.Models;

namespace Questore.Data.Interfaces
{
    public interface IAdminDAO
    {
        public Admin GetAdmin(int id);
    }
}
