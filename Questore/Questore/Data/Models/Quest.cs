namespace Questore.Data.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reward { get; set; }
        public Category Category { get; set; }
        public string ImageUrl { get; set; }
    }
}