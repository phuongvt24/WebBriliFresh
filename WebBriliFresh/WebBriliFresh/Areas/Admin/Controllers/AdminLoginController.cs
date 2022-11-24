using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using WebBriliFresh.Areas.Admin.Models;
using WebBriliFresh.Common;
using WebBriliFresh.Models.DAO;
using System;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminLoginController : Controller
    {
        public IActionResult Index()
        {

            return View("Index");
        }
        [HttpPost]
        public IActionResult Login(LoginModel model) {

            if (ModelState.IsValid)
            {

                if (ModelState.IsValid)
                {
                    var dao = new Admin_Dao();
                    var result = dao.Login(model.UserName, model.PassWord);
                    if (result == 1)
                    {
                        var emp_id = dao.getInfo(model.UserName);
                        var session = new AdminLogin();

                        session.EmpId = (int)emp_id;
                        session.UserId = dao.getItem(model.UserName).UserId;
                        //HttpContext context = HttpContext.Current;
                        HttpContext.Session.SetInt32(AdminSession.ADMIN_SESSION_USERID, session.UserId);
                        HttpContext.Session.SetInt32(AdminSession.ADMIN_SESSION_EMPID, session.EmpId);
                        return RedirectToAction("Index", "Home", new
                        {
                            UserID = session.UserId,
                            EmpID = session.EmpId
                        });
                        //return RedirectToAction("Index", "Home");
                        
                         

                    }
                    else if (result == 0)
                    {
                        ModelState.AddModelError("", "Tài khoản không phải là tài khoản Admin");
                    }
                    else if (result == -1) {
                        ModelState.AddModelError("", "Mật khẩu không đúng, Vui lòng kiểm tra lại.");

                    }
                    else if (result == -2)
                    {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");

                    }

                }
            }
            return View("Index");
        }
    }
}
