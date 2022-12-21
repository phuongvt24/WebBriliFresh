using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;
using WebBriliFresh.Utils;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerDiscountOrdersController : Controller
    {
        private readonly BriliFreshDbContext _context;
        private readonly IEmailSender _emailSender;

        public CustomerDiscountOrdersController(BriliFreshDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender; 
        }

        // GET: CustomerDiscountOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerDiscountOrders.Include(a=>a.Customer).Include(b=>b.DiscountOrder).ToListAsync());
        }

        // GET: CustomerDiscountOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerDiscountOrders == null)
            {
                return NotFound();
            }

            var customerDiscountOrder = await _context.CustomerDiscountOrders
                .FirstOrDefaultAsync(m => m.DiscountOrdersDisId == id);
            if (customerDiscountOrder == null)
            {
                return NotFound();
            }

            return View(customerDiscountOrder);
        }

        // GET: CustomerDiscountOrders/Create
        public IActionResult Create()
        {
            ViewData["CusId"] = _context.Customers.Include(c=>c.Reward).ToList();
            ViewData["DisId"] = new SelectList(_context.DiscountOrders, "DisId", "DisCode");
            return View();
        }

        // POST: CustomerDiscountOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            IFormCollection form = HttpContext.Request.Form;
            int DiscountOrdersDisId = Int32.Parse(form["DiscountOrdersDisId"].ToString().Trim());
            List<string?> CustomersId = form["Customers[]"].ToList();
            DiscountOrder discount = _context.DiscountOrders.FirstOrDefault(a => a.DisId == DiscountOrdersDisId);

            foreach(string CustomerId in CustomersId)
            {
                CustomerDiscountOrder customerDiscountOrder = new CustomerDiscountOrder();
                int cusId = Int32.Parse(CustomerId);
                Customer customer = await _context.Customers.Include(a=>a.User).FirstOrDefaultAsync(a => a.CusId == cusId);
                customerDiscountOrder.DiscountOrdersDisId = DiscountOrdersDisId;
                customerDiscountOrder.CustomersCusId = cusId;
                if (ModelState.IsValid)
                {
                    _context.Add(customerDiscountOrder);
                }
                await _emailSender.SendEmailAsync(customer.User.Email, "Mã giảm giá dành tặng bạn!", discount.DisCode);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: CustomerDiscountOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerDiscountOrders == null)
            {
                return NotFound();
            }

            var customerDiscountOrder = await _context.CustomerDiscountOrders.FindAsync(id);
            if (customerDiscountOrder == null)
            {
                return NotFound();
            }
            return View(customerDiscountOrder);
        }

        // POST: CustomerDiscountOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountOrdersDisId,CustomersCusId,IsDeleted")] CustomerDiscountOrder customerDiscountOrder)
        {
            if (id != customerDiscountOrder.DiscountOrdersDisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerDiscountOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerDiscountOrderExists(customerDiscountOrder.DiscountOrdersDisId))
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
            return View(customerDiscountOrder);
        }

        // GET: CustomerDiscountOrders/Delete/5
        [HttpGet("Delete/{DiscountOrderDisId}/{DiscountOrderCusId}")]
        public async Task<IActionResult> Delete(int? DiscountOrderDisId, int? DiscountOrderCusId)
        {
            if (DiscountOrderDisId == null || _context.CustomerDiscountOrders == null)
            {
                return NotFound();
            }

            var customerDiscountOrder = await _context.CustomerDiscountOrders
                .FirstOrDefaultAsync(m => m.DiscountOrdersDisId == DiscountOrderDisId && m.CustomersCusId == DiscountOrderCusId);
            if (customerDiscountOrder == null)
            {
                return NotFound();
            }

            return View(customerDiscountOrder);
        }

        // POST: CustomerDiscountOrders/Delete/5
        [HttpPost("Delete/{DiscountOrderDisId}/{DiscountOrderCusId}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? DiscountOrderDisId, int? DiscountOrderCusId)
        {
            if (_context.CustomerDiscountOrders == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.CustomerDiscountOrders'  is null.");
            }
            var customerDiscountOrder = await _context.CustomerDiscountOrders
                .FirstOrDefaultAsync(m => m.DiscountOrdersDisId == DiscountOrderDisId && m.CustomersCusId == DiscountOrderCusId);
            if (customerDiscountOrder != null)
            {
                _context.CustomerDiscountOrders.Remove(customerDiscountOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerDiscountOrderExists(int id)
        {
            return _context.CustomerDiscountOrders.Any(e => e.DiscountOrdersDisId == id);
        }
    }
}
