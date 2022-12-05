using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Common;
using WebBriliFresh.Models;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]
    public class HomeController : Controller
    {
        public int s_userid;
        public int s_empid;
        public int getUserID()
        {
            return s_userid;
        }
        public int getEmpID()
        {
            return s_empid;
        }
       
        public IActionResult Index(int UserID, int EmpID)
        {
            BriliFreshDbContext db = new BriliFreshDbContext();
            HttpContext.Session.SetInt32("ADMIN_SESSION_USERID", UserID);
            HttpContext.Session.SetInt32("ADMIN_SESSION_EMPID", EmpID);
            //this.s_userid = UserID;
            //this.s_empid = EmpID;
            //var employee = (from emp in db.Employees where emp.UserId == UserID select emp).FirstOrDefault();

            //var f_name = employee.FirstName;
            //var l_name = employee.LastName;

            //HttpContext.Session.SetInt32(AdminSession.ADMIN_SESSION_USERID, UserID);
            //HttpContext.Session.SetInt32(AdminSession.ADMIN_SESSION_EMPID, EmpID);

            var employee = (from emp in db.Employees where emp.UserId == UserID select emp).FirstOrDefault();


            string? f_name = employee.FirstName==null?"":employee.FirstName;
            string? l_name = employee.LastName == null ? "" : employee.LastName; 
            HttpContext.Session.SetString("ADMIN_SESSION_FIRSTNAME", f_name);
            HttpContext.Session.SetString("ADMIN_SESSION_LASTNAME", l_name);
            var u_avatar = (from user in db.Users where user.UserId == UserID select user).FirstOrDefault();
            string? avatar = u_avatar.Avatar == null ? null : u_avatar.Avatar;
            if (avatar != null)
            {
                HttpContext.Session.SetString("ADMIN_SESSION_AVATAR", avatar);
            }
            else { HttpContext.Session.SetString("ADMIN_SESSION_AVATAR", ""); }



            //HttpContext.Session.SetString(AdminSession.ADMIN_SESSION_FIRSTNAME, f_name);
            //HttpContext.Session.SetString(AdminSession.ADMIN_SESSION_LASTNAME, l_name);

            return View();
        }
    }
}
