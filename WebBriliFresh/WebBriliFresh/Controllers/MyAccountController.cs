using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(IFormFile photo)
        {


            IFormCollection form = HttpContext.Request.Form;
            int CusId = Int32.Parse(form["CusId"]);
            int UserId = Int32.Parse(form["UserId"]);
            int RewardId = Int32.Parse(form["RewardId"]);
            int Gender = Int32.Parse(form["Gender"]);

            string FirstName = form["FirstName"].ToString().Trim();
            string LastName = form["LastName"].ToString().Trim();
            string Phone = form["Phone"].ToString().Trim();
            string Email = form["Email"].ToString().Trim();

            try
            {
                Customer customer = new Customer();
                customer.CusId = CusId;
                customer.UserId = UserId;
                customer.Email = Email;
                customer.RewardId = RewardId;
                customer.FirstName = FirstName;
                customer.LastName = LastName;
                customer.RewardId = RewardId;
                customer.Gender = Gender;

                String name = LastName + " " + FirstName;
                HttpContext.Session.SetString("CUS_SESSION_CUSNAME", name);
                _context.Update(customer);

                if (photo != null)
                {
                    string picfilename = DoPhotoUpload(photo);
                    User user = await _context.Users.FindAsync(UserId);

                    if(user.Avatar != null)
                    {
                        DeleteOldAvatar(user.Avatar);
                    }

                    user.Avatar = picfilename;
                    if (await TryUpdateModelAsync<User>(
                        user,
                        "user",
                        s => s.Avatar))
                    {
                        // EF will detect the change and update only the column that has changed.
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("CUS_SESSION_AVATAR", user.Avatar);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AccountInfo));

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(CusId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CusId == id);
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


        private void DeleteOldAvatar(string fname)
        {
            try
            {
                string fullpath = Path.Combine(_env.WebRootPath, "MyAccountAssets/UserPhotos/" + fname);
                // Check if file exists with its full path    
                if (System.IO.File.Exists(fullpath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(Path.Combine(fullpath));
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
        }

        private string DoPhotoUpload(IFormFile photo)
        {
            string fext = Path.GetExtension(photo.FileName);
            string uname = Guid.NewGuid().ToString();
            string fname = uname + fext;
            string fullpath = Path.Combine(_env.WebRootPath, "MyAccountAssets/UserPhotos/" + fname);
            using (FileStream fs = new(fullpath, FileMode.Create))
            {
                photo.CopyTo(fs);
            }
            return fname;
        }
    }
}
