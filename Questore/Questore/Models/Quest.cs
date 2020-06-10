using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questore.Models
{
    public class Quest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Reward { get; set; }

        public string ImageUrl { get; set; }
    }
}
