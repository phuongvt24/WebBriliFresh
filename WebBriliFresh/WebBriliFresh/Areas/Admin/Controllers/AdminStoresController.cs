using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebBriliFresh.Models;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminStoresController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public AdminStoresController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminStores
        public async Task<IActionResult> Index()
        {
            var stores = _context.Stores.Where(x => x.isDeleted == 0);
            return View(await stores.ToListAsync());
        }

        [HttpPost, ActionName("SearchIndex")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchIndex(int? city)
        {
            var stores = _context.Stores.Where(x => x.IsDeleted == 0);
            if (city != null)
            {
                stores = stores.Where(x => x.City == city.ToString());
            }
            return View(await stores.ToListAsync());
        }

        // GET: Admin/AdminStores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stores == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Admin/AdminStores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminStores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreId,City,District,Ward,SpecificAddress,isDeleted")] Store store)
        {
            if (ModelState.IsValid)
            {
                store.isDeleted = 0;
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Admin/AdminStores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stores == null)
            {
                return NotFound();
            }

            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        // POST: Admin/AdminStores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreId,City,District,Ward,SpecificAddress")] Store store)
        {
            if (id != store.StoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    store.isDeleted = 0;
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.StoreId))
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
            return View(store);
        }

        // GET: Admin/AdminStores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stores == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Admin/AdminStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stores == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.Stores'  is null.");
            }
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                //_context.Stores.Remove(store);
                store.isDeleted = 1;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
          return _context.Stores.Any(e => e.StoreId == id);
        }

    }
}
