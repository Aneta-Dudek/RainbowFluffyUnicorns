using System.ComponentModel.DataAnnotations;

namespace Questore.Data.Dtos
{
    public class Login
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "E-mail:")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}