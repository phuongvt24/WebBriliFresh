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
                var dao = new UserDAO();
                var result = dao.Login(model.UserName, model.PassWord);


                //Admin
                if (result == 3)
                {
                    var emp_id = dao.getEmployeeInfo(model.UserName);
                    var session = new AdminLogin
                    {
                        EmpId = (int)emp_id,
                        UserId = dao.getItem(model.UserName).UserId
                    };

                    HttpContext.Session.SetInt32("EMP_SESSION_EMPID", (int)emp_id);

                    var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Role, "3"),
                            new Claim(ClaimTypes.NameIdentifier, emp_id.ToString()),

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

                //User
                else if (result == 1)
                {
                    var cusId = dao.getCustomerInfo(model.UserName);
                    var session = new CustomerLogin();

                    HttpContext.Session.SetInt32("CUS_SESSION_CUSID", (int)cusId);

                    //HttpContext context = HttpContext.Current;
                    var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Role, "1"),
                            new Claim(ClaimTypes.NameIdentifier, cusId.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
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

                //Employee
                else if (result == 2)
                {
                    var emp_id = dao.getEmployeeInfo(model.UserName);
                    var session = new AdminLogin
                    {
                        EmpId = (int)emp_id,
                        UserId = dao.getItem(model.UserName).UserId
                    };
                    var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Role, "2"),
                            new Claim(ClaimTypes.NameIdentifier, emp_id.ToString()),

                    };
     
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

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
                //mat khau sai
                else if (result == -1)
                    ModelState.AddModelError("", "Mật khẩu không đúng, Vui lòng kiểm tra lại.");
                //ten dang khong ton tai
                else
                    ModelState.AddModelError("", "Tên đăng nhập không tồn tại, Vui lòng kiểm tra lại.");


            }
            return View("Index");
        }
    }
}
