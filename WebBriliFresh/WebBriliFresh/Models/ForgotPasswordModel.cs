using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
