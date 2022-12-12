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

namespace WebBriliFresh.Controllers
{
    public class UserLogin : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly BriliFreshDbContext _context;

        public UserLogin(IUserAuthenticationService authService, BriliFreshDbContext context)
        {
            this._authService = authService;
            _context = context; 
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


            bool isAdmin = User.IsInRole("ADMIN");
            bool isEmployee = User.IsInRole("EMPLOYEE");
            bool isCustomer = User.IsInRole("CUSTOMER");

            if (result.StatusCode == 1 && (isAdmin || isEmployee))
            {

                var empID = (from item in _context.Employees
                             where item.UserId == Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
                             select item.EmpId).First();
                return RedirectToAction("Index", "Home", new
                {
                    area = "Admin",
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    empID = empID
                });
            }
            else if (result.StatusCode == 1 && isCustomer)
            {
                var cusID = (from item in _context.Customers
                             where item.UserId == Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
                             select item.CusId).First();

                return RedirectToAction("Index", "Home", new
                {
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    cusID = cusID
                });
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

    }
}
