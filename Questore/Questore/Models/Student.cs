using System.Collections.Generic;

namespace Questore.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Coolcoins { get; set; }

        public int Experience { get; set; }

        public string ImageUrl { get; set; }

        public Title Title { get; set; }

        public IEnumerable<Class> Classes { get; set; }

        public IEnumerable<Team> Teams { get; set; }

        public IEnumerable<Artifact> Artifacts { get; set; }
        
        public IEnumerable<Detail> Details { get; set; }
    }
}
