using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminEmployeesController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public AdminEmployeesController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminEmployees
        public async Task<IActionResult> Index()
        {
            var briliFreshDbContext = _context.Employees.Include(e => e.Store).Include(e => e.User);
            return View(await briliFreshDbContext.ToListAsync());
        }

        // GET: Admin/AdminEmployees/Details/5
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


        public class AddressStore { 
            public int id { get; set; }
            public string address { get; set; }


            public AddressStore(int Id, string address)
            {
                this.id = Id;
                this.address = address;

            }
        }

        // GET: Admin/AdminEmployees/Create
        public IActionResult Create()
        {
            List<string> a = _context.Stores.Select(x => x.City).ToList();
            List<string> b = _context.Stores.Select(x => x.District).ToList();
            List<string> d = _context.Stores.Select(x => x.Ward).ToList();

            List<int> c = _context.Stores.Select(x => x.StoreId).ToList();

            List<AddressStore> addresses = new List<AddressStore>();

            
            for (int i =0; i<a.Count; i++)
            {
                addresses.Add(new AddressStore(c[i], c[i] + ", "+d[i]+", "+ b[i] + ", " + a[i]));
            }
            ViewData["AddressStore"] = new SelectList(addresses, "id", "address");

            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City");
             ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Admin/AdminEmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpId,UserId,StoreId,FirstName,LastName,Gender,City,District,Ward,SpecificAddress,StartDate,EndDate,Phone,Email,UserName,UserPassword")] Employee employee)
        {
            if (ModelState.IsValid)
            {

                var model = new User();
                model.UserName = employee.UserName;
                model.UserPassword = employee.UserPassword;
                model.UserRole = 2;
                _context.Add(model);
                await _context.SaveChangesAsync();
                int userid = model.UserId;
                employee.UserId = userid;
                employee.IsDeleted = 0;
                _context.Add(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City", employee.StoreId);

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", employee.UserId);
            return View(employee);
        }

        // GET: Admin/AdminEmployees/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City", employee.StoreId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", employee.UserId);
            return View(employee);
        }

        // POST: Admin/AdminEmployees/Edit/5
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City", employee.StoreId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", employee.UserId);
            return View(employee);
        }

        // GET: Admin/AdminEmployees/Delete/5
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

        // POST: Admin/AdminEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);

            var user = await _context.Users.Where(x => x.UserId == employee.UserId).FirstOrDefaultAsync();
            if (employee != null)
            {
                employee.IsDeleted = 1;
                user.IsDeleted = 1;

                _context.Employees.Update(employee);
                _context.Users.Update(user);
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