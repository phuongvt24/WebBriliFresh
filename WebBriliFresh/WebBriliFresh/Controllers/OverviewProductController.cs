using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;

namespace WebBriliFresh.Controllers
{
    public class OverviewProductController : Controller
    {
        private readonly BriliFreshDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notifyService { get; }
        public OverviewProductController(BriliFreshDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;

        }
        //Tổng quan 3 loại danh mục
        public IActionResult FishAndMeat()
        {
            return View();
        }

        //[Authorize(Policy = "LoggedIn")]
        public IActionResult Fruit()
        {

            return View();
        }

        public IActionResult Vegetable()
        {
            return View();
        }

        //List sản phẩm chính
        public async Task<IActionResult> ListFishAndMeat()
        {
            var products = _context.Products.Include(s => s.Type).Include(p => p.ProductImages).Include(a => a.Stocks).Include(z=>z.DiscountProducts).Where(x => x.IsDeleted == 0);
            return View(await products.ToListAsync());
        }


        public IActionResult ListFruit()
        {
            return View();
        }

        public IActionResult ListVegetable()
        {
            return View();
        }

        //chi tiết từng sản phẩm
        public IActionResult DetailFishAndMeat()
        {
            return View();
        }
        public IActionResult DetailFruit()
        {
            return View();
        }

        public IActionResult DetailVegetable()
        {
            return View();
        }


    }
}
