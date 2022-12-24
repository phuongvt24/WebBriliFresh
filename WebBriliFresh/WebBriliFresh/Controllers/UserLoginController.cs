using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using WebBriliFresh.Models;
using WebBriliFresh.Common;
using WebBriliFresh.Models.DAO;
using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using WebBriliFresh.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebBriliFresh.Utils;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Asn1.Ocsp;

namespace WebBriliFresh.Controllers
{
    public class UserLogin : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly BriliFreshDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public UserLogin(IUserAuthenticationService authService, BriliFreshDbContext context, UserManager<User> userManager, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _authService = authService;
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model);

            if (result.StatusCode == 1)
            {
                User user = await _userManager.FindByNameAsync(model.UserName);


                if(user.IsDeleted != 1)
                {
                    int? role = user.UserRole;

                    if (role == 3 || role == 2)
                    {
                        var empID = (from item in _context.Employees
                                     where item.UserId == user.Id
                                     select item.EmpId).First();

                        HttpContext.Session.SetInt32("ADMIN_SESSION_USERID", user.Id);
                        HttpContext.Session.SetInt32("ADMIN_SESSION_EMPID", empID);

                        return RedirectToAction("Index", "Home", new
                        {
                            area = "Admin",
                        });
                    }
                    else if (role == 1)
                    {
                        var cusID = (from item in _context.Customers
                                     where item.UserId == user.Id
                                     select item.CusId).First();

                        Customer currentCustomer = _context.Customers.FirstOrDefault(x => x.CusId == cusID)!;
                        String name = currentCustomer.LastName + " " + currentCustomer.FirstName;


                        HttpContext.Session.SetInt32("CUS_SESSION_USERID", user.Id);
                        HttpContext.Session.SetInt32("CUS_SESSION_CUSID", cusID);
                        HttpContext.Session.SetString("CUS_SESSION_CUSNAME", name);
                        HttpContext.Session.SetString("CUS_SESSION_AVATAR", user.Avatar!);
                        HttpContext.Session.SetString("CUS_SESSION_EMAIL", user.Email!);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); //chua lam trang loi

                    }
                }
                else { return RedirectToAction("Index", "Home"); }//User bi xoa (Employee) //chua lam trang loi
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View("Index");
            }


        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Role = "Customer";
            var result = await this._authService.RegisterAsync(model);


            if (result.StatusCode == 1)
            {
                User user = await _userManager.FindByNameAsync(model.Username);

                Reward reward = new Reward();
                _context.Add(reward);
                await _context.SaveChangesAsync();

                int newRewardId = _context.Rewards.Max(p => p.RewardId);

                Customer cus = new Customer();
                cus.FirstName = model.FirstName;
                cus.LastName = model.LastName;
                cus.Gender = model.Gender;
                cus.Email = model.Email;
                cus.Phone = model.Phone;
                cus.UserId = user.Id;
                cus.RewardId = newRewardId;

                _context.Add(cus);
                await _context.SaveChangesAsync();


                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);


                var confirmationLink = Url.Action(nameof(ConfirmEmail), "UserLogin", new { token, email = user.Email }, Request.Scheme);

                string body = string.Empty;
                //using streamreader for reading my htmltemplate   

                string fullpath = Path.Combine(_env.WebRootPath, "MyAccountAssets/ConfirmationEmail.html");
                using (StreamReader reader = new StreamReader(fullpath))

                {

                    body = reader.ReadToEnd();

                }

                body = body.Replace("{confirmationLink}", confirmationLink); //replacing the required things  

                await _emailSender.SendEmailAsync(user.Email, "Xác nhận email", body);

                result.Message = "Tạo người dùng thành công với đầy đủ thông tin";
                return RedirectToAction(nameof(SuccessRegistration));
            }


            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? nameof(Index) : "Error");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();
            HttpContext.Session.Remove("CUS_SESSION_USERID");
            HttpContext.Session.Remove("CUS_SESSION_CUSID");
            HttpContext.Session.Remove("CUS_SESSION_CUSID");
            HttpContext.Session.Remove("CUS_SESSION_CUSNAME");
            HttpContext.Session.Remove("CUS_SESSION_AVATAR");
            HttpContext.Session.Remove("CUS_SESSION_EMAIL");

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/UserLogin/ChangePassword", Name = "ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            IFormCollection form = HttpContext.Request.Form;

            String CurrentPassword = form["CurrentPassword"].ToString().Trim();
            String NewPassword = form["NewPassword"].ToString().Trim();
            String PasswordConfirm = form["PasswordConfirm"].ToString().Trim();

            ChangePasswordModel model = new ChangePasswordModel();
            model.NewPassword = NewPassword;
            model.PasswordConfirm = PasswordConfirm;
            model.CurrentPassword = CurrentPassword;

            if (!ModelState.IsValid)
                return RedirectToAction("ChangePass_1", "MyAccount");
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("ForgetPass_4", "MyAccount");
            }
            TempData["msg"] = result.Message;
            return RedirectToAction("ChangePass_1", "MyAccount");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword()
        {
            IFormCollection form = HttpContext.Request.Form;
            string Email = form["Email"].ToString().Trim();
            var user = await _userManager.FindByEmailAsync(Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ResetPassword), "UserLogin", new { token, email = user.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Đặt lại mật khẩu", confirmationLink);
            return RedirectToAction("ForgetPass_2", "MyAccount");
        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            TempData["token"] = token;
            TempData["email"] = email;
            return RedirectToAction("ForgetPass_3", "MyAccount");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword()
        {
            IFormCollection form = HttpContext.Request.Form;
            string email = form["Email"].ToString().Trim();
            string password = form["Password"].ToString().Trim();
            string confirmPassword = form["ConfirmPassword"].ToString().Trim();
            string token = form["Token"].ToString().Trim();
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
            resetPasswordModel.Token = token;
            resetPasswordModel.Email = email;
            resetPasswordModel.Password = password;
            resetPasswordModel.ConfirmPassword = confirmPassword;
            var result = await _authService.ResetPasswordAsync(resetPasswordModel);
            //if(result.StatusCode == 1)
            //{
            //    RedirectToAction("ForgetPass_4", "MyAccount");
            //}
            return RedirectToAction("LogOut", "UserLogin");
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyUserName(string UserName)
        {
            User exist = _userManager.FindByNameAsync(UserName).Result;

            if (exist == null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string email)
        {
            User exist = _userManager.FindByEmailAsync(email).Result;

            if (exist == null)
            {
                return Json(true);
            }

            return Json(false);
        }
        //[AllowAnonymous]
        //public async Task<IActionResult> RegisterAdmin()
        //{
        //    RegistrationModel model = new RegistrationModel
        //    {
        //        Username = "admin",
        //        Email = "admin@gmail.com",
        //        Password = "Admin123!",
        //        Role = "admin",
        //        UserRole = 3
        //    };
        //    var result = await this._authService.RegisterAsync(model);
        //    return Ok(result);
        //}

        //[AllowAnonymous]
        //public async Task<IActionResult> RegisterCustomer()
        //{
        //    RegistrationModel model = new RegistrationModel
        //    {
        //        Username = "quynhchi",
        //        Email = "quynhchi@gmail.com",
        //        Password = "Quynhchi123!",
        //        Role = "customer",
        //        UserRole = 1
        //    };
        //    var result = await this._authService.RegisterAsync(model);
        //    return Ok(result);
        //}

        //[AllowAnonymous]
        //public async Task<IActionResult> RegisterEmployee()
        //{
        //    RegistrationModel model = new RegistrationModel
        //    {
        //        Username = "employee",
        //        Email = "employee@gmail.com",
        //        Password = "Employee123!",
        //        Role = "employee",
        //        UserRole = 2
        //    };
        //    var result = await this._authService.RegisterAsync(model);
        //    return Ok(result);
        //}
    }
}
