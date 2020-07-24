using Questore.Data.Models;
using System.Collections.Generic;

namespace Questore.Data.Dtos
{
    public class StudentDto : UserDto
    {
        public int Coolcoins { get; set; }
        public int Experience { get; set; }
        public string ImageUrl { get; set; }
        public string PhotoId { get; set; }
        public Title Title { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<Artifact> Artifacts { get; set; }
        public IEnumerable<StudentDetail> Details { get; set; }

        public StudentDto()
        {
            Role = "student";
        }
    }
}