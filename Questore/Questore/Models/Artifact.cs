﻿namespace Questore.Models
{
    public class Artifact
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public Category Category { get; set; }

        public int Price { get; set; }

        public bool IsUsed { get; set; }
    }
}