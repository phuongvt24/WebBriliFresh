using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using WebBriliFresh.Common;
using WebBriliFresh.Models;

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


        public IActionResult Index()
        {
            int a = _context.Stores.Where(x => x.IsDeleted == 0).Select(c => c.StoreId).FirstOrDefault();
            HttpContext.Session.SetInt32(CommonConstants.SessionStoreId, a);
            return View();
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