using System;
using System.ComponentModel.DataAnnotations;

namespace Questore.Data.Dtos
{
    [Serializable]
    public class DetailDto
    {
        [Required] [MinLength(3)] public string Name { get; set; }

        [Required] [MinLength(3)] public string Content { get; set; }

        public int StudentId { get; set; }
    }
}