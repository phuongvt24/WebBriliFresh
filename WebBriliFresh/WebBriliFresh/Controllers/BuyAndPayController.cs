using Microsoft.AspNetCore.Mvc;

namespace WebBriliFresh.Controllers
{
    public class BuyAndPayController : Controller
    {
        public IActionResult CartInfoCheck()
        {
            return View();
        }
        public IActionResult CartInfo()
        {
            return View();
        }

        public IActionResult DeliveryInfo()
        {
            return View();
        }

        public IActionResult DeliveryInfoLogin()
        {
            return View();
        }

        public IActionResult PayInfo()
        {
            return View();
        }
    }
}
