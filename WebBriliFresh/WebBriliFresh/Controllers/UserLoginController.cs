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

namespace WebBriliFresh.Controllers
{
    public class UserLoginController : Controller
    {
        public IActionResult Index()
        {

            return View("Index");
        }
        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return LocalRedirect("/");
        }


        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {

            if (ModelState.IsValid)
            {

                if (ModelState.IsValid)
                {
                    var dao = new UserDAO();
                    var result = dao.Login(model.UserName, model.PassWord);
                    if (result == 1)
                    {
                        var emp_id = dao.getEmployeeInfo(model.UserName);
                        var session = new AdminLogin();

                        session.EmpId = (int)emp_id;
                        session.UserId = dao.getItem(model.UserName).UserId;
                        //HttpContext context = HttpContext.Current;
                        var claims = new List<Claim>() {
                            new Claim("Admin", "Admin"),
                    };
                        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                        //var principal = new ClaimsPrincipal(identity);
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = false,

                        };
                        await HttpContext.SignInAsync(
                             CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(identity),
                                authProperties);

                        return RedirectToAction("Index", "Home", new
                        {
                            Area = "Admin",
                            UserID = session.UserId,
                            EmpID = session.EmpId
                        });

                    }
                    else if (result == 3)
                    {
                        var cusId = dao.getCustomerInfo(model.UserName);
                        var session = new CustomerLogin();

                        session.CusId = (int)cusId;
                        session.UserId = dao.getItem(model.UserName).UserId;
                        //HttpContext context = HttpContext.Current;
                        var claims = new List<Claim>() {
                            new Claim("Customer", "Customer"),
                    };
                        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                        //var principal = new ClaimsPrincipal(identity);
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = false,

                        };
                        await HttpContext.SignInAsync(
                             CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(identity),
                                authProperties);

                        return RedirectToAction("Index", "Home", new
                        {
                            UserID = session.UserId,
                            CusID = session.CusId
                        });
                    }
                    else if (result == 2)
                    {
                        ModelState.AddModelError("", "Mật khẩu không đúng, Vui lòng kiểm tra lại.");

                    }
                    else if (result == null)
                    {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");

                    }

                }
            }
            return View("Index");
        }
    }
}
