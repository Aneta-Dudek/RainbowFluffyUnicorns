using Questore.Data.Models;

namespace Questore.Data.Interfaces
{
    public interface IAdminDAO
    {
        public Admin GetAdmin(int id);
    }
}