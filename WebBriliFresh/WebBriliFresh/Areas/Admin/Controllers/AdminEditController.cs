using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]

    public class AdminEditController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public AdminEditController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminEdit
        public async Task<IActionResult> Index()
        {
            var briliFreshDbContext = _context.Employees.Include(e => e.Store).Include(e => e.User);
            return View(await briliFreshDbContext.ToListAsync());
        }

        // GET: Admin/AdminEdit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Store)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Admin/AdminEdit/Create
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Admin/AdminEdit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpId,UserId,StoreId,FirstName,LastName,Gender,City,District,Ward,SpecificAddress,StartDate,EndDate,Phone,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreId", employee.StoreId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", employee.UserId);
            return View(employee);
        }

        // GET: Admin/AdminEdit/Edit/5
        public async Task<IActionResult> Edit(int? id , int id2)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreId", employee.StoreId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", employee.UserId);
            //HttpContext.Session.SetInt32("ADMIN_SESSION_USERID", (int)id);
            //HttpContext.Session.SetInt32("ADMIN_SESSION_EMPID", id2);
            return View(employee);
        }

        // POST: Admin/AdminEdit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpId,UserId,StoreId,FirstName,LastName,Gender,City,District,Ward,SpecificAddress,StartDate,EndDate,Phone,Email")] Employee employee)
        {
            if (id != employee.EmpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmpId))
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
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreId", employee.StoreId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", employee.UserId);
            return View(employee);
        }

        // GET: Admin/AdminEdit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Store)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Admin/AdminEdit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.EmpId == id);
        }
    }
}
