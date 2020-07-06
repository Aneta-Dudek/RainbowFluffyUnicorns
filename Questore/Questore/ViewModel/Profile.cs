using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Questore.Dtos;
using Questore.Models;

namespace Questore.ViewModel
{
    public class Profile
    {
        public Student Student { get; set; }

        public DetailDto Detail { get; set; }
    }
}
