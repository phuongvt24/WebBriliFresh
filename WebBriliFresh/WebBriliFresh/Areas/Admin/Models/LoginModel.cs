using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string PassWord { get; set; }
        
    }
}
