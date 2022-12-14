using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBriliFresh.Models;

namespace WebBriliFresh.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly BriliFreshDbContext _context;

        public MyAccountController(UserManager<User> userManager, BriliFreshDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Policy = "LoggedIn")]
        public IActionResult AccountInfo()
        {
            int cusID = (int)HttpContext.Session.GetInt32("CUS_SESSION_CUSID");

            Customer currentCustomer = _context.Customers.FirstOrDefault(x => x.CusId == cusID);

            return View(currentCustomer);
        }

        public IActionResult MyNotice()
        {
            return View();
        }
        public IActionResult ManageOrder()
        {
            return View();
        }
        public IActionResult ManageAddress()
        {
            return View();
        }
        public IActionResult ManageFeedback()
        {
            return View();
        }
        public IActionResult ChangePass_1()
        {
            return View();
        }
      
        public IActionResult ForgetPass_1()
        {
            return View();
        }
        public IActionResult ForgetPass_2()
        {
            return View();
        }
        public IActionResult ForgetPass_3()
        {
            return View();
        }
        public IActionResult ForgetPass_4()
        {
            return View();
        }

        public IActionResult ChangeMail_1()
        {
            return View();
        }

        public IActionResult ChangeMail_2()
        {
            return View();
        }

        public IActionResult ChangeMail_3()
        {
            return View();
        }

    }
}
