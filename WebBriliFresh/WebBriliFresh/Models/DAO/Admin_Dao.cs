using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using WebBriliFresh.Areas.Admin.Controllers;
using System.Linq;
namespace WebBriliFresh.Models.DAO
{
    public class Admin_Dao
    {
        BriliFreshDbContext db = null;
        public Admin_Dao() {
            db = new BriliFreshDbContext();
            
        }

        public User getItem(string? user_name) {
            return db.Users.FirstOrDefault(x => x.UserName == user_name);
        }

        public List<User> getList_Admin() {
            return db.Users.Where(x => x.UserRole == 1).ToList();
        }
        public int? getInfo(string? user_name)
        {

            var empID = (from item in db.Employees
                         where item.UserId == getItem(user_name).UserId
                         select item.EmpId).First();
            //data = data.Where(x => x.UserId == getItem(user_name).UserId);
            return empID;
        }

        public int Login(string user_name, string pass) {

            var user = db.Users.FirstOrDefault(x=>x.UserName==user_name);

            if (user == null) {
                return -2; //Tai khoan khong ton tai
            }
            else {
                if (user.UserRole != 1)
                {
                    return 0; //Khong phai admin
                }
                else {
                    if (user.UserPassword == pass)
                    { 
                        return 1; //Thanh cong
                    }
                    else {
                        return -1; //Sai mat khau
                    }
                }
            }

        }


    }

}
