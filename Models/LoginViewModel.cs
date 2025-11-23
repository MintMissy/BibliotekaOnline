using System.ComponentModel.DataAnnotations;

namespace TEST.Models
{
    public class LoginViewModel
    {
        public string Login { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
