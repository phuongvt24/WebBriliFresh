using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBriliFresh.Common;
using WebBriliFresh.Models;
using WebBriliFresh.Models.DTO;
using WebBriliFresh.Helpers;
using Microsoft.EntityFrameworkCore;

namespace WebBriliFresh.Controllers
{
    public class BuyAndPayController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public BuyAndPayController(BriliFreshDbContext context)
        {
            _context = context;
        }

        public List<ShoppingCartViewModel> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.SessionCart);
                if(data == null)
                {
                    data = new List<ShoppingCartViewModel>();
                }
                return data;
            }
        }
        //
        public IActionResult AddToCart(int proId, int storeid, decimal saleprice, string type = "normal")
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.productId == proId);
            if(item == null)
            {
                    item = new ShoppingCartViewModel
                    {
                        productId = proId,
                        quantity = 1,
                        storeId = storeid,
                        saleprice = saleprice
                    };
                    myCart.Add(item);      
            }
            else
            {
                item.quantity++;
            }
            HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
            if(type == "ajax")
            {
                return Json(new
                {
                    quantity = Carts.Sum(p=>p.quantity)
                });
            }
            return RedirectToAction("ListFishAndMeat", "OverviewProduct");
        }



        public IActionResult Delete(int proId)
        {
            var myCart = Carts;
            if (Carts != null)
            {
                myCart.RemoveAll(x => x.productId == proId);
                HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
                return Json(new
                {
                    quantity = Carts.Sum(p => p.quantity)
                }) ;
            }
            return Json(new
            {
                status = false
            });
        }

        public IActionResult Update(int proId, int quantity)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.productId == proId);
            item.quantity = quantity;
            HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
            return Json(new
            {
                quantity = Carts.Sum(p => p.quantity)
            });
        }



        public IActionResult CartInfoCheck()
        {
            return View(Carts);
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
