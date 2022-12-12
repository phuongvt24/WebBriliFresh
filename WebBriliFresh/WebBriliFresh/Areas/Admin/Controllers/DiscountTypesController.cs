using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;
using static WebBriliFresh.Areas.Admin.Controllers.AdminEmployeesController;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountTypesController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public DiscountTypesController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DiscountTypes
        public async Task<IActionResult> Index()
        {
            var briliFreshDbContext = _context.DiscountTypes.Include(d => d.Type);
            return View(await briliFreshDbContext.ToListAsync());
        }

        // GET: Admin/DiscountTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DiscountTypes == null)
            {
                return NotFound();
            }

            var discountType = await _context.DiscountTypes
                .Include(d => d.Type)
                .FirstOrDefaultAsync(m => m.DisId == id);
            if (discountType == null)
            {
                return NotFound();
            }

            return View(discountType);
        }

        // GET: Admin/DiscountTypes/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId");
            ViewData["MainType"] = new SelectList(_context.Types, "MainType", "MainType");
            ViewData["SubType"] = new SelectList(_context.Types, "SubType", "SubType");
            return View();
        }

        // POST: Admin/DiscountTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisId,TypeId,Value,StartDate,EndDate,Status")] DiscountType discountType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", discountType.TypeId);
            return View(discountType);
        }

        // GET: Admin/DiscountTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DiscountTypes == null)
            {
                return NotFound();
            }

            var discountType = await _context.DiscountTypes.FindAsync(id);
            if (discountType == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", discountType.TypeId);
            return View(discountType);
        }

        // POST: Admin/DiscountTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DisId,TypeId,Value,StartDate,EndDate,Status")] DiscountType discountType)
        {
            if (id != discountType.DisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountTypeExists(discountType.DisId))
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
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", discountType.TypeId);
            return View(discountType);
        }

        // GET: Admin/DiscountTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DiscountTypes == null)
            {
                return NotFound();
            }

            var discountType = await _context.DiscountTypes
                .Include(d => d.Type)
                .FirstOrDefaultAsync(m => m.DisId == id);
            if (discountType == null)
            {
                return NotFound();
            }

            return View(discountType);
        }

        // POST: Admin/DiscountTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DiscountTypes == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.DiscountTypes'  is null.");
            }
            var discountType = await _context.DiscountTypes.FindAsync(id);
            if (discountType != null)
            {
                _context.DiscountTypes.Remove(discountType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountTypeExists(int id)
        {
          return _context.DiscountTypes.Any(e => e.DisId == id);
        }
    }

    internal class DirectoryType
    {
    }
}
