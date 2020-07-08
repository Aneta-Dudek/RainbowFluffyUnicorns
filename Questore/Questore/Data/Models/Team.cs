using System.Collections.Generic;

namespace Questore.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<Student> Students { get; set; }
    }
}