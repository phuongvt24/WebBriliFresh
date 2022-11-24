using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebBriliFresh.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public int s_userid;
        public int s_empid;
        public int getUserID(){
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
            this.s_userid = UserID;
            this.s_empid = EmpID;
            var employee = (from emp in db.Employees where emp.UserId == UserID select emp).FirstOrDefault();
            var emp_model = new Employee()
            {
                
                FirstName = employee.FirstName,
                LastName = employee.LastName, 
                
            };
           

            return View(emp_model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult BackHome(string returnUrl)
        //{

        //    //return RedirectToAction("Index", "Home");
        //    return Redirect(returnUrl);

        //}
    }
}
