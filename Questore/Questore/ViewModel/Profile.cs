using Microsoft.AspNetCore.Http;
using Questore.Data.Dtos;
using Questore.Data.Models;

namespace Questore.ViewModel
{
    public class Profile
    {
        public Student Student { get; set; }
        public DetailDto Detail { get; set; }
        public FormFile FormFile { get; set; }
    }
}
