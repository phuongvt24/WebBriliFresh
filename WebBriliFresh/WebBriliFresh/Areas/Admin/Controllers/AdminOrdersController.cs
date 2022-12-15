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

    [Authorize(Policy = "Employee")]
    public class AdminOrdersController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public AdminOrdersController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminOrders
        public async Task<IActionResult> Index()
        {
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(x => x.IsDeleted == 0), "StoreId", "StoreId");
            var briliFreshDbContext = _context.Orders.Include(o => o.Add).Include(o => o.Dis).Include(o => o.Store).Include(o => o.Trans).Include(o => o.Cus); 
            return View(await briliFreshDbContext.ToListAsync());
        }

        // GET: Admin/AdminOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Add)
                .Include(o => o.Dis)
                .Include(o => o.Store)
                .Include(o => o.Trans)
                .Include(o => o.Cus)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            var orderdetail = await _context.OrderDetails
                .Include(o => o.Pro)
                .Where(m => m.OrderId == id).ToListAsync();
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        /*
        // GET: Admin/AdminOrders/Create
        public IActionResult Create()
        {
            ViewData["AddId"] = new SelectList(_context.Addresses, "AddId", "AddId");
            ViewData["DisId"] = new SelectList(_context.DiscountOrders, "DisId", "DisCode");
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City");
            ViewData["TransId"] = new SelectList(_context.Transports, "TransId", "TransId");
            return View();
        }

        // POST: Admin/AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,AddId,TransId,DisId,StoreId,OrderDate,SubTotal,OrderTotal,PayBy,Status,CusId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddId"] = new SelectList(_context.Addresses, "AddId", "AddId", order.AddId);
            ViewData["DisId"] = new SelectList(_context.DiscountOrders, "DisId", "DisCode", order.DisId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City", order.StoreId);
            ViewData["TransId"] = new SelectList(_context.Transports, "TransId", "TransId", order.TransId);
            return View(order);
        }
        */
        // GET: Admin/AdminOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Add)
                .Include(o => o.Dis)
                .Include(o => o.Store)
                .Include(o => o.Trans)
                .Include(o => o.Cus)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            var orderdetail = await _context.OrderDetails
                .Include(o => o.Pro)
                .Where(m => m.OrderId == id).ToListAsync();
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Status, OrderId,AddId,TransId,DisId,StoreId,OrderDate,SubTotal,OrderTotal,PayBy,Status,CusId,Trans,Trans.TransId,Trans.ShippingDate,Trans.Type,Trans.Transporter,Trans.Fee,Trans.Status")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);

                    if (order.Trans.Status == 6)
                    {
                        using (var context = new BriliFreshDbContext())
                        {
                            Customer cUpd = context.Customers.Include(c => c.Reward).FirstOrDefault(c => c.CusId == order.CusId);
                            if (cUpd != null && cUpd.RewardId != null && cUpd.Reward != null)
                            {
                                cUpd.Reward.Point += order.OrderTotal;
                                if (cUpd.Reward.Point >= 9000000)
                                {
                                    cUpd.Reward.CusType = 1;
                                }
                                else if (cUpd.Reward.Point >= 4000000)
                                {
                                    cUpd.Reward.CusType = 2;
                                }
                                else
                                {
                                    cUpd.Reward.CusType = 3;
                                }
                            }
                            context.SaveChanges();
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(order);
            }
            return View(order);
        }
        /*
        // GET: Admin/AdminOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Add)
                .Include(o => o.Dis)
                .Include(o => o.Store)
                .Include(o => o.Trans)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ProName"] = new SelectList(_context.Products, "ProId", "ProName", order.OrderDetails.Quantity);
            ViewData["ProPrice"] = new SelectList(_context.Products, "ProId", "Price", order.AddId);
            ViewData["ProUnit"] = new SelectList(_context.Products, "ProId", "Unit", order.AddId);
            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool OrderExists(int id)
        {
          return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
