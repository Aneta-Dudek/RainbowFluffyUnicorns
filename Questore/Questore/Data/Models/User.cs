namespace Questore.Data.Models
{
    public abstract class User
    {
        public int CredentialsId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; protected set; }
    }
}