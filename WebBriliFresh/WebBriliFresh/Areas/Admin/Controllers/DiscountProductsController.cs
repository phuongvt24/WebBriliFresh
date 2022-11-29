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
    public class DiscountProductsController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public DiscountProductsController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DiscountProducts
        public async Task<IActionResult> Index()
        {
            var briliFreshDbContext = _context.DiscountProducts.Include(d => d.Pro);
            return View(await briliFreshDbContext.ToListAsync());
        }

        // GET: Admin/DiscountProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DiscountProducts == null)
            {
                return NotFound();
            }

            var discountProduct = await _context.DiscountProducts
                .Include(d => d.Pro)
                .FirstOrDefaultAsync(m => m.DisId == id);
            if (discountProduct == null)
            {
                return NotFound();
            }

            return View(discountProduct);
        }

        // GET: Admin/DiscountProducts/Create
        public IActionResult Create()
        {
            ViewData["ProId"] = new SelectList(_context.Products, "ProId", "ProName");
            return View();
        }

        // POST: Admin/DiscountProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisId,ProId,Value,StartDate,EndDate,Status")] DiscountProduct discountProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProId"] = new SelectList(_context.Products, "ProId", "ProName", discountProduct.ProId);
            return View(discountProduct);
        }

        // GET: Admin/DiscountProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DiscountProducts == null)
            {
                return NotFound();
            }

            var discountProduct = await _context.DiscountProducts.FindAsync(id);
            if (discountProduct == null)
            {
                return NotFound();
            }
            ViewData["ProId"] = new SelectList(_context.Products, "ProId", "ProName", discountProduct.ProId);
            return View(discountProduct);
        }

        // POST: Admin/DiscountProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DisId,ProId,Value,StartDate,EndDate,Status")] DiscountProduct discountProduct)
        {
            if (id != discountProduct.DisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountProductExists(discountProduct.DisId))
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
            ViewData["ProId"] = new SelectList(_context.Products, "ProId", "ProName", discountProduct.ProId);
            return View(discountProduct);
        }

        // GET: Admin/DiscountProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DiscountProducts == null)
            {
                return NotFound();
            }

            var discountProduct = await _context.DiscountProducts
                .Include(d => d.Pro)
                .FirstOrDefaultAsync(m => m.DisId == id);
            if (discountProduct == null)
            {
                return NotFound();
            }

            return View(discountProduct);
        }

        // POST: Admin/DiscountProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DiscountProducts == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.DiscountProducts'  is null.");
            }
            var discountProduct = await _context.DiscountProducts.FindAsync(id);
            if (discountProduct != null)
            {
                _context.DiscountProducts.Remove(discountProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountProductExists(int id)
        {
          return _context.DiscountProducts.Any(e => e.DisId == id);
        }
    }
}
