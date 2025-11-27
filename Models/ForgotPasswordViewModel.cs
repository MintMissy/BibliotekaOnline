using System.ComponentModel.DataAnnotations;

namespace TEST.Models
{
public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Adres email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Wprowadź poprawny adres email.")]
    public string Email { get; set; } = null!;
}

}
