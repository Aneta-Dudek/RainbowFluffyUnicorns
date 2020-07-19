using System.Collections.Generic;

namespace Questore.Data.Models
{
    public class Student : User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Coolcoins { get; set; }
        public int Experience { get; set; }
        public string ImageUrl { get; set; }
        public string PhotoId { get; set; }
        public Title Title { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<Artifact> Artifacts { get; set; }
        public IEnumerable<StudentDetail> Details { get; set; }
        public Student()
        {
            Role = "student";
        }
    }
}
