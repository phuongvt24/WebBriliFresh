
using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Vui lòng nhập Username")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập phải có ít nhất 6 kí tự.", MinimumLength = 6)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự.", MinimumLength = 8)]
        [Display(Name = "Password")]
        public string PassWord { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không trùng khớp.")]
        public string PassWordConfirm { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Họ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Tên")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Hãy chọn Giới tính")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b",
            ErrorMessage = "Số điện thoại không hợp lệ")]
        public string NumberPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email của bạn")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
       


    }


}
