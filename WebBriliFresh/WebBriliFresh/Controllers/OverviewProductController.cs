using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebBriliFresh.Controllers
{
    public class OverviewProductController : Controller
    {
        //Tổng quan 3 loại danh mục
        public IActionResult FishAndMeat()
        {
            return View();
        }

        [Authorize(Policy = "LoggedIn")]
        public IActionResult Fruit()
        {

            return View();
        }

        public IActionResult Vegetable()
        {
            return View();
        }

        //List sản phẩm chính
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
