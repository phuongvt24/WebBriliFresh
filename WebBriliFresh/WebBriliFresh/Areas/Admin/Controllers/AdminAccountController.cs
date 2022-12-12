using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;
using static WebBriliFresh.Areas.Admin.Controllers.AdminEmployeesController;
namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]

    public class AdminAccountController : Controller
    {
        private readonly BriliFreshDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notifyService { get; }

        public AdminAccountController(BriliFreshDbContext context, IWebHostEnvironment hostEnvironment, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
            this._hostEnvironment = hostEnvironment;

        }

        // GET: Admin/AdminAccount
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(x => x.Employees).Include(a => a.Customers).Where(s => s.IsDeleted == 0).ToListAsync());
        }

        // GET: Admin/AdminAccount/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/AdminAccount/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminAccount/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,UserPassword,UserRole,IsDeleted,Avatar")] User user)
        {
            if (ModelState.IsValid)
            {
                user.IsDeleted = 0;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/AdminAccount/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            var user2 = await _context.Users.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == id);
            if (user2 == null)
            {
                return NotFound();
            }
            return View(user2);
        }

        // POST: Admin/AdminAccount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,ImageFile,UserName,UserPassword,UserRole,Avatar")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            var a = _context.Users.Where(x => x.Id == id).Select(p => p.Avatar).ToList();
            user.Avatar = a[0];

            if (ModelState.IsValid)
            {
                user.IsDeleted = 0;
                if (user.ImageFile != null)
                {
                    try
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
                        string extension = Path.GetExtension(user.ImageFile.FileName);
                        user.Avatar = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                        string path = Path.Combine(wwwRootPath + "/ImageUser/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await user.ImageFile.CopyToAsync(fileStream);
                        }
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Edit));
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit));
            }
            await _context.SaveChangesAsync();
            _context.Update(user);
            return View(user);
        }

        // GET: Admin/AdminAccount/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Include(x=>x.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/AdminAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            var emp = await _context.Employees.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsDeleted = 1;
                emp.IsDeleted = 1;
                _context.Users.Update(user);
                _context.Employees.Update(emp);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        [HttpPost]
        public IActionResult Avtphoto(int id)
        {
            var avt = _context.Users.Where(x => x.Id == id).Select(p => p.Avatar).FirstOrDefault();
            if (avt == null)
            {
                return Json(
                    new
                    {
                        success = 0,
                        message = "Không thấy ảnh sản phẩm"

                    }
               );
            }

            var photo = "/" + avt;
            return Json(new
            {
                success = 1,
                urlavt = photo

            });
        }

    }
}