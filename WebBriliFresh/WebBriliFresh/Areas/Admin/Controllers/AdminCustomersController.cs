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

    public class AdminCustomersController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public AdminCustomersController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminCustomers
        public async Task<IActionResult> Index(int? Custype)
        {
            var briliFreshDbContext = _context.Customers.Include(c => c.Reward).Include(c => c.User);
            return View(await briliFreshDbContext.ToListAsync());
            //if (RewardID == null)
            //{
            //    ViewData["RewardId"] = new SelectList(_context.Rewards, "RewardId", "RewardId");
            //    var briliFreshDbContext = _context.Customers.Include(c => c.Reward).Include(c => c.User);
            //    return View(await briliFreshDbContext.ToListAsync());
            //}
            //else
            //{
            //    ViewData["RewardId"] = new SelectList(_context.Rewards, "RewardId", "RewardId");
            //    var briliFreshDbContext = _context.Customers.Include(c => c.Reward).Include(c => c.User).Where(c => c.RewardId == RewardID);
            //    return View(await briliFreshDbContext.ToListAsync());
            //}
            //var data = new object[]{
            //    new{
            //        ten = "Vàng",
            //        giatri = 1
            //    },
            //    new{
            //        ten = "Bạc",
            //        giatri = 2
            //    },
            //    new{
            //        ten = "Đồng",
            //        giatri = 3
            //    }
            //    };
            //    if (Custype == null)
            //    {
            //        ViewData["list"] = new SelectList(data, "giatri", "ten");
            //        var briliFreshDbContext = _context.Rewards.Include(c => c.Customers);
            //        return View(await briliFreshDbContext.ToListAsync());
            //    }
            //    else
            //    {
            //        ViewData["list"] = new SelectList(data, "giatri", "ten");
            //        var briliFreshDbContext = _context.Rewards.Include(c => c.Customers).Where(c => c.CusType == Custype);
            //        return View(await briliFreshDbContext.ToListAsync());
            //    }
        }

            // GET: Admin/AdminCustomers/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null || _context.Customers == null)
                {
                    return NotFound();
                }

                var customer = await _context.Customers
                    .Include(c => c.Reward)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(m => m.CusId == id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }

            // GET: Admin/AdminCustomers/Create
            public IActionResult Create()
            {
                ViewData["RewardId"] = new SelectList(_context.Rewards, "RewardId", "RewardId");
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
                return View();
            }

            // POST: Admin/AdminCustomers/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("CusId,UserId,RewardId,FirstName,LastName,Gender,Phone,Email")] Customer customer)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["RewardId"] = new SelectList(_context.Rewards, "RewardId", "RewardId", customer.RewardId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
                return View(customer);
            }

            // GET: Admin/AdminCustomers/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null || _context.Customers == null)
                {
                    return NotFound();
                }

                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                ViewData["RewardId"] = new SelectList(_context.Rewards, "RewardId", "RewardId", customer.RewardId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
                return View(customer);
            }

            // POST: Admin/AdminCustomers/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("CusId,UserId,RewardId,FirstName,LastName,Gender,Phone,Email")] Customer customer)
            {
                if (id != customer.CusId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CustomerExists(customer.CusId))
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
                ViewData["RewardId"] = new SelectList(_context.Rewards, "RewardId", "RewardId", customer.RewardId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
                return View(customer);
            }

            // GET: Admin/AdminCustomers/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null || _context.Customers == null)
                {
                    return NotFound();
                }

                var customer = await _context.Customers
                    .Include(c => c.Reward)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(m => m.CusId == id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }

            // POST: Admin/AdminCustomers/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                if (_context.Customers == null)
                {
                    return Problem("Entity set 'BriliFreshDbContext.Customers'  is null.");
                }
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool CustomerExists(int id)
            {
                return _context.Customers.Any(e => e.CusId == id);
            }
        }
    }
