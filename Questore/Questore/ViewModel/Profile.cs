using Microsoft.AspNetCore.Http;
using Questore.Dtos;
using Questore.Models;

namespace Questore.ViewModel
{
    public class Profile
    {
        public Student Student { get; set; }

        public DetailDto Detail { get; set; }

        public FormFile FormFile { get; set; }
    }
}
