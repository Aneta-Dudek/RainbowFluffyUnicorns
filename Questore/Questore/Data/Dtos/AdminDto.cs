namespace Questore.Data.Dtos
{
    public class AdminDto : UserDto
    {
        public AdminDto()
        {
            Role = "admin";
        }
    }
}