using Questore.Data.Models;
using Questore.Dtos;

namespace Questore.Data.Interfaces
{
    public interface IAuthentication
    {
        public User Authenticate(Login login);
    }
}
