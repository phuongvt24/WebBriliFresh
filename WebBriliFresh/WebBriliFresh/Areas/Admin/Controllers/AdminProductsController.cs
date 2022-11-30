using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly BriliFreshDbContext _context;
        public INotyfService _notifyService { get; }
        public AdminProductsController(BriliFreshDbContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/AdminProducts
        public async Task<IActionResult> Index()
        {
            var products = _context.Products.Include(s => s.Type).Where(x => x.IsDeleted == 0);
            return View(await products.ToListAsync());
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["TypeId1"] = new SelectList(_context.Types.Where(x => x.MainType == "Rau củ"), "TypeId", "SubType");
            ViewData["TypeId2"] = new SelectList(_context.Types.Where(x => x.MainType == "Thịt cá"), "TypeId", "SubType");
            ViewData["TypeId3"] = new SelectList(_context.Types.Where(x => x.MainType == "Trái cây 4 mùa"), "TypeId", "SubType");
            ViewData["MainType"] = new SelectList(_context.Types.GroupBy(p => p.MainType).Select(x => new { MainType = x.Key }), "MainType", "MainType");
            return View();
        }


        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([Bind("ProId,ProName,Price,TypeId,Source,StartDate,Des,Unit,IsDeleted,File,Files")] Product product)
            {

            if (ModelState.IsValid)
            {   
                product.IsDeleted = 0;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", product.TypeId);
            ViewData["TypeId1"] = new SelectList(_context.Types.Where(x => x.MainType == "Rau củ"), "TypeId", "SubType");
            ViewData["TypeId2"] = new SelectList(_context.Types.Where(x => x.MainType == "Thịt cá"), "TypeId", "SubType");
            ViewData["TypeId3"] = new SelectList(_context.Types.Where(x => x.MainType == "Trái cây 4 mùa"), "TypeId", "SubType");


            ViewData["MainType"] = new SelectList(_context.Types.GroupBy(p => p.MainType)
                                                                .Select(x => new { MainType = x.Key }), "MainType", "MainType");
            return  View();
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", product.TypeId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProId,ProName,Price,TypeId,Source,StartDate,Des,Unit,IsDeleted")] Product product)
        {
            if (id != product.ProId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProId))
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
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", product.TypeId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProId == id);
        }


        public class UploadListFile
        {
            [Required(ErrorMessage = "Hãy chọn tệp hình ảnh để tải!")]
            [DataType(DataType.Upload)]
            [FileExtensions(Extensions ="png,jpg,jpeg,gif")]
            [Display(Name = "Chọn tệp để tải")]
            public List<IFormFile>? Files { get; set; }
            
        }


    //    [HttpGet]
    //    public IActionResult UploadPhoto(int id)
    //    {
    //        var product = _context.Products.Where(e => e.ProId == id)
    //                                        .Include(p => p.ProductImages)
    //                                        .FirstOrDefault();
    //        if (product == null)
    //        {
    //            return NotFound("Không có sản phẩm");
    //        }
    //        ViewData["product"] = product;  

    //        return View(new UploadListFile());
    //    }

    //    [HttpPost, ActionName("UploadPhoto")]
    //    public async Task< IActionResult> UploadPhotoAsync(int id, UploadListFile lf)
    //    {
    //        var product = _context.Products.Where(e => e.ProId == id)
    //                                        .Include(p=>p.ProductImages)
    //                                        .FirstOrDefault();
    //        if (product == null)
    //        {
    //            return NotFound("Không có sản phẩm  akhd ádks");
    //        }
    //        ViewData["product"] = product;


    //        foreach (var formFile in lf)
    //        {
    //            if (formFile.Length > 0)
    //            {
    //                using (var stream = new FileStream(filePath, FileMode.Create))
    //                {
    //                    await formFile.CopyToAsync(stream);
    //                }
    //            }
    //        }



    //        if (f != null)
    //        {
    //            var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
    //                       + Path.GetExtension(f.Files.FileName);
    //            var file2 = Path.Combine("Uploads","Images","ProductImages",file1);

    //            using (var filesream = new FileStream(file2, FileMode.Create))
    //            {
    //                await f.Files.CopyToAsync(filesream);
    //            }

    //            _context.Add(new ProductImage()
    //            {
    //                ProId = product.ProId,
    //                ImgData = file1
    //            });
    //            await _context.SaveChangesAsync();
    //        }
    //        return View(new UploadOneFile());
    //    }
    }
}
