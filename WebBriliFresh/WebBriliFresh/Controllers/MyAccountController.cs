using Microsoft.AspNetCore.Mvc;

namespace WebBriliFresh.Controllers
{
    public class MyAccountController : Controller
    {
        public IActionResult AccountInfo()
        {
            return View();
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
