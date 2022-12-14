﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        [EmailAddress]
        [Remote(action: "VerifyEmail", controller: "UserLogin", ErrorMessage = "Email đã tồn tại")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        [Remote(action: "VerifyUserName", controller: "UserLogin", ErrorMessage = "Tên đăng nhập đã tồn tại")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        [RegularExpression("^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Mật khẩu tối thiểu 6 ký tự và có ít nhất 1 chữ viết hoa,1 chữ viết thường, 1 ký tự đặc biệt và 1 số")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string? Role { get; set; } = "Customer";
        public int? UserRole { get; set; } = 1;
    }
}
