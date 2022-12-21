using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Web.Helpers;
using WebBriliFresh.Common;
using WebBriliFresh.Helpers;
using WebBriliFresh.Models;
using WebBriliFresh.Models.DTO;
using System.Web;

namespace WebBriliFresh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BriliFreshDbContext _context;

        public HomeController(ILogger<HomeController> logger, BriliFreshDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<ShoppingCartViewModel> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
                if (data == null)
                {
                    data = new List<ShoppingCartViewModel>();
                }
                return data;
            }
        }

        public async Task<IActionResult> changeid(int storeid)
        {
            HttpContext.Session.SetInt32(CommonConstants.SessionStoreId, storeid);
            return  RedirectToAction("Index");
        }


        public async Task<IActionResult> Index()
        {
            var a = _context.Stores.Where(x => x.IsDeleted == 0);
            if (HttpContext.Session.GetInt32(CommonConstants.SessionStoreId) == null)
            {
                var random_storeid = _context.Stores.Select(x => x.StoreId).FirstOrDefault();
                HttpContext.Session.SetInt32(CommonConstants.SessionStoreId, random_storeid);
            } 

            return View(await a.ToListAsync());
        }

        [Authorize(Policy = "CustomerOnly")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}