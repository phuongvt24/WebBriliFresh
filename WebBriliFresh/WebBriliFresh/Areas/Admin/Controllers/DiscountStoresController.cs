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
    public class DiscountStoresController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public DiscountStoresController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DiscountStores
        public async Task<IActionResult> Index()
        {
            var briliFreshDbContext = _context.DiscountStores.Include(d => d.Store);
            return View(await briliFreshDbContext.ToListAsync());
        }

        // GET: Admin/DiscountStores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DiscountStores == null)
            {
                return NotFound();
            }

            var discountStore = await _context.DiscountStores
                .Include(d => d.Store)
                .FirstOrDefaultAsync(m => m.DisId == id);
            if (discountStore == null)
            {
                return NotFound();
            }

            return View(discountStore);
        }

        // GET: Admin/DiscountStores/Create
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City");
            return View();
        }

        // POST: Admin/DiscountStores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisId,StoreId,Value,StartDate,EndDate,Status")] DiscountStore discountStore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountStore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City", discountStore.StoreId);
            return View(discountStore);
        }

        // GET: Admin/DiscountStores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DiscountStores == null)
            {
                return NotFound();
            }

            var discountStore = await _context.DiscountStores.FindAsync(id);
            if (discountStore == null)
            {
                return NotFound();
            }
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City", discountStore.StoreId);
            return View(discountStore);
        }

        // POST: Admin/DiscountStores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DisId,StoreId,Value,StartDate,EndDate,Status")] DiscountStore discountStore)
        {
            if (id != discountStore.DisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountStore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountStoreExists(discountStore.DisId))
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
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "City", discountStore.StoreId);
            return View(discountStore);
        }

        // GET: Admin/DiscountStores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DiscountStores == null)
            {
                return NotFound();
            }

            var discountStore = await _context.DiscountStores
                .Include(d => d.Store)
                .FirstOrDefaultAsync(m => m.DisId == id);
            if (discountStore == null)
            {
                return NotFound();
            }

            return View(discountStore);
        }

        // POST: Admin/DiscountStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DiscountStores == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.DiscountStores'  is null.");
            }
            var discountStore = await _context.DiscountStores.FindAsync(id);
            if (discountStore != null)
            {
                _context.DiscountStores.Remove(discountStore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountStoreExists(int id)
        {
          return _context.DiscountStores.Any(e => e.DisId == id);
        }
    }
}
