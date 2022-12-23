using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Common;
using WebBriliFresh.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Employee")]
    public class HomeController : Controller
    {
        public int s_userid;
        public int s_empid;

        //public string ConnectionString { get; private set; }

        public int getUserID()
        {
            return s_userid;
        }
        public int getEmpID()
        {
            return s_empid;
        }
       
        public IActionResult Index()
        {
            BriliFreshDbContext db = new BriliFreshDbContext();

            int UserID = (int)HttpContext.Session.GetInt32("ADMIN_SESSION_USERID");
            int empID = (int)HttpContext.Session.GetInt32("ADMIN_SESSION_EMPID");
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

            var u_avatar = (from user in db.Users where user.Id == UserID select user).FirstOrDefault();
            string avatar = u_avatar.Avatar;

            if (avatar != null)
            {
                HttpContext.Session.SetString("ADMIN_SESSION_AVATAR", avatar);
            }
            else { HttpContext.Session.SetString("ADMIN_SESSION_AVATAR", ""); }



            //HttpContext.Session.SetString(AdminSession.ADMIN_SESSION_FIRSTNAME, f_name);
            //HttpContext.Session.SetString(AdminSession.ADMIN_SESSION_LASTNAME, l_name);

            return View();
        }
        /*
        public JsonResult DashBoardcount()
        {
            try
            {
                string[] DashBoardcount = new string[2];    

                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Select count(*) as thanhvien, (select count(*) from Customer where userid is null) as khachvanglai from customer where userid is not null", con);
                DataTable dt = new DataTable();
                SqlDataAdapter cmd1 = new SqlDataAdapter(cmd);
                cmd1.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    DashBoardcount[0] = "0";
                    DashBoardcount[1] = "0";
                }
                else
                {
                    DashBoardcount[0] = dt.Rows[0]["thanhvien"].ToString();
                    DashBoardcount[1] = dt.Rows[1]["khachvanglai"].ToString();
                }
                return Json(DashBoardcount);

                    }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
    }
}
