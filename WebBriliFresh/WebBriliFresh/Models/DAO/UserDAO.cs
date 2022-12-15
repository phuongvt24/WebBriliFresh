using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using WebBriliFresh.Areas.Admin.Controllers;
using System.Linq;
namespace WebBriliFresh.Models.DAO
{
    public class UserDAO
    {
        BriliFreshDbContext db = null;
        public UserDAO() {
            db = new BriliFreshDbContext();

        }

        public User getItem(string? user_name) {
            return db.Users.FirstOrDefault(x => x.UserName == user_name);
        }

        public List<User> getList_Admin() {
            return db.Users.Where(x => x.UserRole == 1).ToList();
        }
        public int? getEmployeeInfo(string? user_name)
        {

            var empID = (from item in db.Employees
                         where item.UserId == getItem(user_name).Id
                         select item.EmpId).First();
            //data = data.Where(x => x.UserId == getItem(user_name).UserId);
            return empID;
        }

        public int? getCustomerInfo(string? user_name)
        {

            var cusID = (from item in db.Customers
                         where item.UserId == getItem(user_name).Id
                         select item.CusId).First();
            return cusID;
        }


        public int? Login(string user_name, string pass) {

            var user = db.Users.FirstOrDefault(x => x.UserName == user_name);

            if (user == null) {
                return 0; //Tai khoan khong ton tai
            }
            else {
                if (user.UserPassword == pass)
                    return user.UserRole; // đúng hết
                else
                    return -1; //mật khẩu sai
            }

        }

        public int checkUserName(string userName) {

            var user = db.Users.FirstOrDefault(x => x.UserName == userName);

            if (user != null)
            {
                return 0; //Ten dang nhap da ton tai
            }
            else{
                return 1; //Hop le;
            }
        }


    }

}
