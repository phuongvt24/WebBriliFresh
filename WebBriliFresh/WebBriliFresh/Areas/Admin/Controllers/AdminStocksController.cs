using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;


namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminStocksController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public AdminStocksController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminStocks
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                ViewData["StoreId"] = new SelectList(_context.Stores.Where(x => x.isDeleted == 0), "StoreId", "StoreId");
                var briliFreshDbContext = _context.Stocks.Include(s => s.Pro).Where(s => s.Pro.IsDeleted == 0).Include(s => s.Store).Where(s => s.Store.isDeleted == 0); ; ;
                return View(await briliFreshDbContext.ToListAsync());
            }
            else
            {
                ViewData["StoreId"] = new SelectList(_context.Stores.Where(x => x.isDeleted == 0), "StoreId", "StoreId");
                var briliFreshDbContext = _context.Stocks.Include(s => s.Pro).Where(s => s.Pro.IsDeleted == 0).Include(s => s.Store).Where(s => s.Store.isDeleted == 0 && s.Store.StoreId == id); ; ;
                return View(await briliFreshDbContext.ToListAsync());
            }
            
        }

        // GET: Admin/AdminStocks/Details/5
        public async Task<IActionResult> Details(int? id, int? id1)
        {
            if (id == null || id1 == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Pro)
                .Include(s => s.Store)
                .FirstOrDefaultAsync(m => m.StoreId == id && m.ProId == id1);
            
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Admin/AdminStocks/Create
        public IActionResult Create(int? id)
        {
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(x => x.isDeleted == 0), "StoreId", "StoreId");
            ViewData["ProId"] = new SelectList(_context.Products.Where(x => x.IsDeleted == 0), "ProId", "ProId");
            return View();
        }

        // POST: Admin/AdminStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreId,ProId,Quantity")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StoreId"] = new SelectList(_context.Stores.Where(x => x.isDeleted == 0), "StoreId", "StoreId");
            ViewData["ProId"] = new SelectList(_context.Products.Where(x => x.IsDeleted == 0), "ProId", "ProId");
            return View(stock);
        }

        // GET: Admin/AdminStocks/Edit/5
        public async Task<IActionResult> Edit(int? id, int? id1)
        {
            if (id == null || id1 == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks.FindAsync(new object[] { id, id1 });
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(x => x.isDeleted == 0), "StoreId", "StoreId");
            ViewData["ProId"] = new SelectList(_context.Products.Where(x => x.IsDeleted == 0), "ProId", "ProId");
            return View(stock);
        }

        // POST: Admin/AdminStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int id1, [Bind("StoreId,ProId,Quantity")] Stock stock)
        {
            if (id != stock.StoreId || id1 != stock.ProId)
            {
                return NotFound();
            }
            _context.Update(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            /*
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.StoreId, stock.ProId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(x => x.isDeleted == 0), "StoreId", "StoreId");
            ViewData["ProId"] = new SelectList(_context.Products.Where(x => x.IsDeleted == 0), "ProId", "ProId");
            return View(stock);
            */
        }

        // GET: Admin/AdminStocks/Delete/5
        public async Task<IActionResult> Delete(int? id, int? id1)
        {
            if (id == null || id1 == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Pro)
                .Include(s => s.Store)
                .FirstOrDefaultAsync(m => m.StoreId == id && m.ProId == id1);
            
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Admin/AdminStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int id1)
        {
            if (_context.Stocks == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.Stocks'  is null.");
            }
            var stock = await _context.Stocks.FindAsync(new object[] { id, id1 });

            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                return View(stock);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id, int id1)
        {
            if (_context.Stocks.Any(e => e.StoreId == id))
            {
                if (_context.Stocks.Any(e => e.ProId == id1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }    
        }
    }
}
