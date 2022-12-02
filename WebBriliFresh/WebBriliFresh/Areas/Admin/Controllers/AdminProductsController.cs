using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebBriliFresh.Models;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly BriliFreshDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notifyService { get; }
        public AdminProductsController(BriliFreshDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;

        }

        // GET: Admin/AdminProducts
        public async Task<IActionResult> Index()
        {
            var products = _context.Products.Include(s => s.Type).Include(p => p.ProductImages).Include(a=>a.Stocks).Where(x => x.IsDeleted == 0);
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
                //return RedirectToAction("UploadPhoto", product);



                if (product.File != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string file1 = "is_avt"+Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                               + Path.GetExtension(product.File.FileName);
                    string file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                    using (var filesream = new FileStream(file2, FileMode.Create))
                    {
                        await product.File.CopyToAsync(filesream);
                    }

                    _context.Add(new ProductImage()
                    {
                        ProId = product.ProId,
                        ImgData = file1
                    });
                    await _context.SaveChangesAsync();
                }

                if (product.Files != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    foreach (var file in product.Files)
                    {
                        string file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                               + Path.GetExtension(file.FileName);
                        string file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                        using (var filesream = new FileStream(file2, FileMode.Create))
                        {
                            await file.CopyToAsync(filesream);
                        }

                        _context.Add(new ProductImage()
                        {
                            ProId = product.ProId,
                            ImgData = file1
                        });
                        await _context.SaveChangesAsync();
                    }

                }

                return RedirectToAction(nameof(Index));

            }
            else
            {
                ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", product.TypeId);
                ViewData["TypeId1"] = new SelectList(_context.Types.Where(x => x.MainType == "Rau củ"), "TypeId", "SubType");
                ViewData["TypeId2"] = new SelectList(_context.Types.Where(x => x.MainType == "Thịt cá"), "TypeId", "SubType");
                ViewData["TypeId3"] = new SelectList(_context.Types.Where(x => x.MainType == "Trái cây 4 mùa"), "TypeId", "SubType");
                ViewData["MainType"] = new SelectList(_context.Types.GroupBy(p => p.MainType).Select(x => new { MainType = x.Key }), "MainType", "MainType");
                return View();
            }
            

        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(s => s.Type)
                                                 .Include(p => p.ProductImages)
                                                 .Include(a => a.Stocks)
                                                 .FirstOrDefaultAsync(i => i.ProId == id.Value);

            if (product == null)
            {
                return NotFound();
            }
            ViewData["TypeId1"] = new SelectList(_context.Types.Where(x => x.MainType == "Rau củ"), "TypeId", "SubType");
            ViewData["TypeId2"] = new SelectList(_context.Types.Where(x => x.MainType == "Thịt cá"), "TypeId", "SubType");
            ViewData["TypeId3"] = new SelectList(_context.Types.Where(x => x.MainType == "Trái cây 4 mùa"), "TypeId", "SubType");
            ViewData["MainType"] = new SelectList(_context.Types.GroupBy(p => p.MainType).Select(x => new { MainType = x.Key }), "MainType", "MainType");
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeId", product.TypeId);
            
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, [Bind("ProId,ProName,Price,TypeId,Source,StartDate,Des,Unit,IsDeleted,File,Files")] Product product)
        {
            if (id != product.ProId)                  
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var arr = _context.ProductImages.Where(x => x.ProId == product.ProId).Select(p => p.ImgId).ToList();

                var arrdata = _context.ProductImages.Where(x => x.ProId == product.ProId).Select(p => p.ImgData).ToList();

                var len = arr.Count;
                string wwwRootPath = _hostEnvironment.WebRootPath;
                try
                {
                    if (product.File != null)
                    {
                        if (len >= 1)
                        {
                            bool isAvt = false;
                            for (int i = 0; i < len; i++)
                            {
                                if (arrdata[i].Contains("is_avt"))
                                {
                                    //string wwwRootPath = _hostEnvironment.WebRootPath;
                                    string file1 = "is_avt" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                                               + Path.GetExtension(product.File.FileName);
                                    string file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                                    using (var filesream = new FileStream(file2, FileMode.Create))
                                    {
                                        await product.File.CopyToAsync(filesream);
                                    }


                                    _context.Update(new ProductImage()
                                    {
                                        ImgId = arr[i],
                                        ProId = product.ProId,
                                        ImgData = file1
                                    });
                                    await _context.SaveChangesAsync();
                                    isAvt = true;
                                }
                            }
                            if (isAvt == false)
                            {
                                //string wwwRootPath = _hostEnvironment.WebRootPath;
                                string file1 = "is_avt" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                                            + Path.GetExtension(product.File.FileName);
                                string file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                                using (var filesream = new FileStream(file2, FileMode.Create))
                                {
                                    await product.File.CopyToAsync(filesream);
                                }

                                _context.Add(new ProductImage()
                                {
                                    ProId = product.ProId,
                                    ImgData = file1
                                });
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            //string wwwRootPath = _hostEnvironment.WebRootPath;
                            string file1 = "is_avt" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                                        + Path.GetExtension(product.File.FileName);
                            string file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                            using (var filesream = new FileStream(file2, FileMode.Create))
                            {
                                await product.File.CopyToAsync(filesream);
                            }

                            _context.Add(new ProductImage()
                            {
                                ProId = product.ProId,
                                ImgData = file1
                            });
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                    if (product.Files != null)
                    {
                        int[] arr_id = new int[10];
                        int isdetail = 0;
                        for(int i = 0; i < len; i++)
                        {
                            if (!(arrdata[i].Contains("is_avt")))
                            {
                                
                                arr_id[isdetail] = arr[i];
                                isdetail++;
                            }
                        }
                        
                        for (int i = 0; i < product.Files.Count; i++)
                        {
                            
                            if (i<isdetail)
                            {
                                
                                string file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                                           + Path.GetExtension(product.Files[i].FileName);
                                string file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                                using (var filesream = new FileStream(file2, FileMode.Create))
                                {
                                    await product.Files[i].CopyToAsync(filesream);
                                }


                                _context.Update(new ProductImage()
                                {
                                    ImgId = arr_id[i],
                                    ProId = product.ProId,
                                    ImgData = file1
                                });
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                string file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                                           + Path.GetExtension(product.Files[i].FileName);
                                string file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                                using (var filesream = new FileStream(file2, FileMode.Create))
                                {
                                    await product.Files[i].CopyToAsync(filesream);
                                }


                                _context.Add(new ProductImage()
                                {
                                    ProId = product.ProId,
                                    ImgData = file1
                                });
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
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


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(Product product)
        {
            var pro = _context.Products.Where(e => e.ProId == product.ProId);
            if (pro == null)
            {
                return NotFound("Không có sản phẩm");
            }
            ViewData["product"] = pro;

            if (product.File != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                           + Path.GetExtension(product.File.FileName);
                var file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                using (var filesream = new FileStream(file2, FileMode.Create))
                {
                    await product.File.CopyToAsync(filesream);
                }

                _context.Add(new ProductImage()
                {
                    ProId = product.ProId,
                    ImgData = file1
                });
                await _context.SaveChangesAsync();
            }

            if (product.Files != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                for(int i = 0; i<= product.Files.Count;i++ ) 
                {
                    var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                           + Path.GetExtension(product.Files[i].FileName);
                    var file2 = Path.Combine(wwwRootPath + "/ImageProduct/", file1);

                    using (var filesream = new FileStream(file2, FileMode.Create))
                    {
                        await product.Files[i].CopyToAsync(filesream);
                    }

                    _context.Add(new ProductImage()
                    {
                        ProId = product.ProId,
                        ImgData = file1
                    });
                    await _context.SaveChangesAsync();
                }

            }
            return View(nameof(Index));
        }
    }
}
