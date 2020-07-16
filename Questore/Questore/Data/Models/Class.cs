using System;
using System.Collections.Generic;

namespace Questore.Data.Models
{
    public class Class
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateStarted { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<Student> Students { get; set; }
    }
}