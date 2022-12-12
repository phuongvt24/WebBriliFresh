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

namespace WebBriliFresh.Controllers
{
    public class UserLogin : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly BriliFreshDbContext _context;
        private readonly UserManager<User> _userManager;


        public UserLogin(IUserAuthenticationService authService, BriliFreshDbContext context, UserManager<User> userManager)
        {
            _authService = authService;
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model);


            User user = await _userManager.FindByNameAsync(model.UserName);

            int? role = user.UserRole;

           
            if (result.StatusCode == 1 && (role == 3 || role == 2))
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
            else if (result.StatusCode == 1 && role == 1)
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
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            model.Role = "Customer";
            var result = await this._authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
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


        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin()
        {
            RegistrationModel model = new RegistrationModel
            {
                Username = "admin",
                Email = "admin@gmail.com",
                Password = "Admin123!",
                Role = "admin",
                UserRole = 3
            };
            var result = await this._authService.RegisterAsync(model);
            return Ok(result);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterCustomer()
        {
            RegistrationModel model = new RegistrationModel
            {
                Username = "quynhchi",
                Email = "quynhchi@gmail.com",
                Password = "Quynhchi123!",
                Role = "customer",
                UserRole = 1
            };
            var result = await this._authService.RegisterAsync(model);
            return Ok(result);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterEmployee()
        {
            RegistrationModel model = new RegistrationModel
            {
                Username = "employee",
                Email = "employee@gmail.com",
                Password = "Employee123!",
                Role = "employee",
                UserRole = 2
            };
            var result = await this._authService.RegisterAsync(model);
            return Ok(result);
        }
    }
}
