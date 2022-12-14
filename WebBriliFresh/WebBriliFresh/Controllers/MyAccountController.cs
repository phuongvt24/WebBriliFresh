using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBriliFresh.Models;

namespace WebBriliFresh.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly BriliFreshDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MyAccountController(BriliFreshDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        private string DoPhotoUpload(IFormFile photo)
        {
            string fext = Path.GetExtension(photo.FileName);
            string uname = Guid.NewGuid().ToString();
            string fname = uname + fext;
            string fullpath = Path.Combine(_env.WebRootPath, "photos/" + fname);
            using (FileStream fs = new(fullpath, FileMode.Create))
            {
                photo.CopyTo(fs);
            }
            return fname;
        }
    }
}
