using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using System.Collections.Generic;
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
                customer.Phone = Phone;

                String name = LastName + " " + FirstName;
                HttpContext.Session.SetString("CUS_SESSION_CUSNAME", name);
                _context.Update(customer);

                if (photo != null)
                {
                    string picfilename = DoPhotoUpload(photo, "MyAccountAssets/UserPhotos/");
                    User user = await _context.Users.FindAsync(UserId);

                    if (user.Avatar != null || user.Avatar != "download.jfif")
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
                        HttpContext.Session.SetString("CUS_SESSION_AVATAR", picfilename);
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
        public async Task<IActionResult> ManageOrder()
        {
            int cusID = (int)HttpContext.Session.GetInt32("CUS_SESSION_CUSID");
            var cusOrders = _context.Orders.Where(c => c.CusId == cusID)
                .Include(a => a.OrderDetails)
                .ThenInclude(cs => cs.Pro)
                .Include(a => a.OrderDetails)
                .ThenInclude(cs=>cs.Pro.ProductImages);
            
            return View(await cusOrders.ToListAsync());
        }
        public async Task<IActionResult> ManageAddress()
        {
            int cusID = (int)HttpContext.Session.GetInt32("CUS_SESSION_CUSID");
            var cusAddresses = _context.Addresses.Where(c => c.CusId == cusID).Include(a => a.Cus);

            return View(await cusAddresses.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress()
        {
            IFormCollection form = HttpContext.Request.Form;
            int cusId = Int32.Parse(form["CusId"]);
            int addId = Int32.Parse(form["AddId"]);
            string city = form["city-select"].ToString().Trim();
            string district = form["district-select"].ToString().Trim();
            string ward = form["ward-select"].ToString().Trim();
            string specificAddress = form["specific-address"].ToString().Trim();
            string isDefault = form["address-default"].ToString().Trim();
            int check;
            bool isCheck = Int32.TryParse(isDefault, out check);


            Address address = await _context.Addresses.FindAsync(addId);
            address.City = city;
            address.Ward = ward;
            address.District = district;
            address.SpecificAddress = specificAddress;
            if (check == 1)
            {
                address.Default = 1;
                var defaultAddress = _context.Addresses.FirstOrDefault(c => c.CusId == cusId && c.Default == 1);
                defaultAddress.Default = 0;
                _context.Update(defaultAddress);
            }


            if (await TryUpdateModelAsync<Address>(
                       address,
                       "address",
                       s => s.City!, s => s.Ward!, s => s.District!, s => s.Default))
            {
                // EF will detect the change and update only the column that has changed.
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(ManageAddress));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress()
        {
            IFormCollection form = HttpContext.Request.Form;
            int cusId = Int32.Parse(form["CusId"]);
            string city = form["city-select"].ToString().Trim();
            string district = form["district-select"].ToString().Trim();
            string ward = form["ward-select"].ToString().Trim();
            string specificAddress = form["specific-address"].ToString().Trim();
            string isDefault = form["address-default"].ToString().Trim();
            int check;
            bool isCheck = Int32.TryParse(isDefault, out check);


            Address address = new Address();
            address.City = city;
            address.Ward = ward;
            address.District = district;
            address.SpecificAddress = specificAddress;
            address.CusId = cusId;

            if (check == 1)
            {
                address.Default = 1;
                var defaultAddress = _context.Addresses.FirstOrDefault(c => c.CusId == cusId && c.Default == 1);
                defaultAddress.Default = 0;
                _context.Update(defaultAddress);
            }
            else
            {
                address.Default = 0;

            }

            _context.Add(address);
           
           await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageAddress));
        }


        [Route("SetAsDefaultAddress/{id}")]
        public async Task<IActionResult> SetAsDefaultAddress(int? id)
        {
            Address address = await _context.Addresses.FindAsync(id);

            Address defaultAddress = _context.Addresses.FirstOrDefault(c => c.CusId == address.CusId && c.Default == 1);
            defaultAddress.Default = 0;
            address.Default = 1;
            _context.Update(defaultAddress);
            _context.Update(address);
            await _context.SaveChangesAsync();
   
            return RedirectToAction(nameof(ManageAddress));
        }

        public async Task<IActionResult> DeleteAddress(int? id)
        {
            Address address = await _context.Addresses.Include(x => x.Orders).FirstOrDefaultAsync(c => c.AddId == id);
            if(address.Orders.Count == 0 && address.Default == 0)
            {
                _context.Remove(address);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(ManageAddress));
        }

        public async Task<IActionResult> ManageFeedback()
        {
            int cusID = (int)HttpContext.Session.GetInt32("CUS_SESSION_CUSID");

            var cusOrders = _context.Orders.Where(c => c.CusId == cusID);
            var deliveredOrders = cusOrders.Where(c => c.Trans.Status == 6);
            var test = deliveredOrders.Include(c => c.OrderDetails.Where(a => a.Pro.Feedbacks.Where(w => w.OrderId == a.OrderId).First() == null))
                                        .ThenInclude(q => q.Pro)
                                        .Include(c => c.OrderDetails.Where(a => a.Pro.Feedbacks.Where(w => w.OrderId == a.OrderId).First() == null))
                                        .ThenInclude(q => q.Pro.ProductImages)
                                        .Include(a => a.Feedbacks)
                                        .Include(a => a.Trans);
            return View(await test.ToListAsync());
        }
        public async Task<IActionResult> OrderDetail()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("MyAccount/GiveFeedback")]
        public async Task<IActionResult> GiveFeedback()
        {

                
            IFormCollection form = HttpContext.Request.Form;
            int proId = Int32.Parse(form["ProId"]);
            int cusId = (int)HttpContext.Session.GetInt32("CUS_SESSION_CUSID");
            int orderId = Int32.Parse(form["OrderId"]);
            string color = form["Color"].ToString().Trim();
            string packaging = form["Packaging"].ToString().Trim();
            string details = form["Details"].ToString().Trim() ?? "Không có ý kiến";
            string message = "Màu sắc: " + color + "\nBao bì: " + packaging + "\nÝ kiến: " +details;
            int star = Int32.Parse(form["star"].ToString().Trim());

            Feedback feedback = new Feedback();
            feedback.ProId = proId;
            feedback.CusId = cusId;
            feedback.OrderId = orderId;
            feedback.Message = message;
            feedback.SendDate = DateTime.Now;
            feedback.Rate = star;
            _context.Add(feedback);
            await _context.SaveChangesAsync();
            
            Feedback newFeedback = _context.Feedbacks.FirstOrDefault(a => a.ProId == proId && a.CusId == cusId && a.OrderId == orderId);
            if (newFeedback != null)
            {
                foreach (var photo in form.Files)
                {
                    string fname = DoPhotoUpload(photo, "MyAccountAssets/FeedbackImage/");
                    FeedbackImage feedbackImage = new FeedbackImage();
                    feedbackImage.ImgData = fname;
                    feedbackImage.FbId = newFeedback.FbId;
                    _context.Add(feedbackImage);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageFeedback));
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
            TempData["Token"] = TempData["token"];
            TempData["Email"] = TempData["email"];

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

        private string DoPhotoUpload(IFormFile photo, string pathToSave)
        {
            string fext = Path.GetExtension(photo.FileName);
            string uname = Guid.NewGuid().ToString();
            string fname = uname + fext;
            string fullpath = Path.Combine(_env.WebRootPath, pathToSave + fname);
            using (FileStream fs = new(fullpath, FileMode.Create))
            {
                photo.CopyTo(fs);
            }
            return fname;
        }
    }
}
