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
using WebBriliFresh.Models.DTO;
using WebBriliFresh.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebBriliFresh.Utils;

namespace WebBriliFresh.Controllers
{
    public class UserLogin : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly BriliFreshDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public UserLogin(IUserAuthenticationService authService, BriliFreshDbContext context, UserManager<User> userManager, IEmailSender emailSender)
        {
            _authService = authService;
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
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
                    HttpContext.Session.SetInt32("CUS_SESSION_USERID", user.Id);
                    HttpContext.Session.SetInt32("CUS_SESSION_EMPID", cusID);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home"); //chua lam trang loi

                }

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
        
                await _emailSender.SendEmailAsync(user.Email, "Xác nhận email", confirmationLink);

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
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyUserName(string UserName)
        {
            var exist = _userManager.FindByNameAsync(UserName);

            if (exist != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string email)
        {
            var exist = _userManager.FindByEmailAsync(email);

            if (exist != null)
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
