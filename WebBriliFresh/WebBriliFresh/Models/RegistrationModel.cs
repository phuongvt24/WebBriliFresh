using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models
{
    public class RegistrationModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        [RegularExpression("^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Mật khẩu tối thiểu 6 ký tự và có ít nhất 1 chữ viết hoa,1 chữ viết thường, 1 ký tự đặc biệt và 1 số")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string? Role { get; set; } = "Customer";
        public int? UserRole { get; set; } = 1;
    }
}
