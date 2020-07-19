using Questore.Data.Dtos;
using Questore.Data.Models;

namespace Questore.Data.Interfaces
{
    public interface IAuthentication
    {
        public User Authenticate(Login login);
    }
}
