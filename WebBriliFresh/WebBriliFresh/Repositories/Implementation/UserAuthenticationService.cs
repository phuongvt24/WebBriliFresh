using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebBriliFresh.Models.DTO;
using WebBriliFresh.Models;
using WebBriliFresh.Repositories.Abstract;
using WebBriliFresh.Migrations;
using Azure.Core;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;

namespace WebBriliFresh.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SignInManager<User> signInManager;
        public UserAuthenticationService(UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "Tên đăng nhập đã tồn tại";
                return status;
            }
            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                UserRole = model.UserRole,
                PhoneNumber = model.Phone,
                IsDeleted = 0,
                UserPassword = model.Password
            };

            if(user.UserRole == 2 || user.UserRole == 3)
            {
                user.EmailConfirmed = true;
            }

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Đăng ký thất bại";
                return status;
            }

            if(user.UserRole == 1)
            await userManager.AddToRoleAsync(user, "Customer");
            else if(user.UserRole == 2)
            await userManager.AddToRoleAsync(user, "Employee");
            else if (user.UserRole == 3)
            await userManager.AddToRoleAsync(user, "Admin");


            status.StatusCode = 1;
            status.Message = "Đăng ký thành công";
            return status;
        }


        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Tên đăng nhập không tồn tại.";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.PassWord))
            {
                status.StatusCode = 0;
                status.Message = "Mật khẩu sai";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.PassWord, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                IdentityResult result = await userManager.AddClaimsAsync(user, authClaims);

                status.StatusCode = 1;
                status.Message = "Đăng nhập thành công";
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "Tài khoản đã bị khóa";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Có lỗi xảy ra khi đăng nhập";
            }

            return status;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();

        }

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        {
            var status = new Status();

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "Người dùng không tồn tại";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Cập nhật mật khẩu mới thành công";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Sự cố đã xảy ra";
                status.StatusCode = 0;
            }
            return status;

        }

        public async Task<Status> ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            var status = new Status();
            var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
                status.Message = "Người dùng không tồn tại";
                status.StatusCode = 0;
                return status;

            }
            var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

            if (!resetPassResult.Succeeded)
            {
                resetPassResult.Errors.ToList().ForEach(error => status.Message += error.Description);
                string tst = status.Message;
                status.StatusCode = 0;
                return status;
            }
            status.Message = "Đặt lại mật khẩu thành công";
            status.StatusCode = 1;
            return status;
        }

        public async Task<User> FindByNameAsync(string username)
        {
            User user = await userManager.FindByNameAsync(username);
            return user;
        }
    }
}
