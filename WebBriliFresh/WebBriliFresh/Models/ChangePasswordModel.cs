using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Mật khẩu tối thiểu 6 ký tự và có ít nhất 1 chữ viết hoa,1 chữ viết thường, 1 ký tự đặc biệt và 1 số")]
        public string? NewPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        [Compare("NewPassword")]
        public string? PasswordConfirm { get; set; }

    }
}
