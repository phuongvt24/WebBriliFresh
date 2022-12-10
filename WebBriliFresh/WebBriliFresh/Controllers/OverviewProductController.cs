using Microsoft.AspNetCore.Mvc;

namespace WebBriliFresh.Controllers
{
    public class OverviewProductController : Controller
    {
        public IActionResult FishAndMeat()
        {
            return View();
        }
        public IActionResult Fruit()
        {
            return View();
        }

        public IActionResult Vegetable()
        {
            return View();
        }

        public IActionResult ListFishAndMeat()
        {
            return View();
        }
        public IActionResult ListFruit()
        {
            return View();
        }

        public IActionResult ListVegetable()
        {
            return View();
        }


    }
}
