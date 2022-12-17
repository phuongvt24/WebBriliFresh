using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBriliFresh.Common;
using WebBriliFresh.Models;
using WebBriliFresh.Models.DTO;
using WebBriliFresh.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Web.Helpers;
using Nancy.Json;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using Nancy;

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
            var item = myCart.SingleOrDefault(p => p.ProductId == proId);
            if(item == null)
            {
                    item = new ShoppingCartViewModel
                    {
                        ProductId = proId,
                        Quantity = 1,
                        StoreId = storeid,
                        SalePrice = saleprice
                    };
                    myCart.Add(item);      
            }
            else
            {
                item.Quantity++;
            }
            HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
            if(type == "ajax")
            {
                return Json(new
                {
                    quantity = Carts.Sum(p=>p.Quantity)
                });
            }
            return RedirectToAction("ListFishAndMeat", "OverviewProduct");
        }



        public IActionResult Delete(int proId)
        {
            var myCart = Carts;
            if (Carts != null)
            {
                myCart.RemoveAll(x => x.ProductId == proId);
                HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
                return Json(new
                {
                    quantity = Carts.Sum(p => p.ProductId)
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
            var item = myCart.SingleOrDefault(p => p.ProductId == proId);
            item.Quantity = quantity;
            HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
            return Json(new
            {
                quantity = Carts.Sum(p => p.Quantity)
            });
        }


        [HttpPost]
        public JsonResult checkout(string checkoutitem )
        {
            var model2 = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(checkoutitem);
            return Json(model2);
        }



        public async Task<IActionResult> Create([Bind("FirstName,Gender,Phone,City,District,Ward,SpecificAddress,Type,StoreId,OrderTotal,SubTotal,PayBy,Status,ListOrder")] CreateOrderModel cre_Ord)
        {
            var order_details = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cre_Ord.ListOrder);
            var cus_id_num = _context.Customers.Where(x => x.Phone == cre_Ord.Phone).Select(x => x.CusId).FirstOrDefault();
            if (cus_id_num != 0)
            {
                var check_address = _context.Addresses.Where(x => x.SpecificAddress == cre_Ord.SpecificAddress)
                                                      .Where(z => z.Ward == cre_Ord.Ward)
                                                      .Where(z => z.District == cre_Ord.District)
                                                      .Where(z => z.City == cre_Ord.City)
                                                      .Select(a=>a.AddId)
                                                      .FirstOrDefault();
                if (check_address != 0)
                {
                    Transport transport = new Transport();
                    transport.ShippingDate = null;
                    transport.Transporter = null;
                    transport.Status = 1;
                    transport.Type = cre_Ord.Type;
                    if (transport.Type == 1)
                    {
                        transport.Fee = 14000;
                    }
                    else
                    {
                        transport.Fee = 32000;
                    }
                    _context.Transports.Add(transport);
                    await _context.SaveChangesAsync();

                    Order order = new Order();
                    order.AddId = check_address;
                    order.CusId = cus_id_num;
                    order.TransId = transport.TransId;
                    order.DisId = null;
                    order.StoreId = cre_Ord.StoreId;
                    order.OrderDate = DateTime.Now;
                    order.SubTotal = cre_Ord.SubTotal;
                    order.OrderTotal = cre_Ord.OrderTotal;
                    order.PayBy = cre_Ord.PayBy;
                    if (cre_Ord.PayBy == 1)
                    {
                        order.Status = 1;
                    }
                    else
                    {
                        order.Status = 0;
                    }
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();


                }
                else
                {
                    var check_default = _context.Addresses.Where(x => x.CusId == cus_id_num).Where(x => x.Default == 1).FirstOrDefault();
                    Address address = new Address();
                    address.CusId = cus_id_num;
                    address.SpecificAddress = cre_Ord.SpecificAddress;
                    address.Ward = cre_Ord.Ward;
                    address.District = cre_Ord.District;
                    address.City = cre_Ord.City;
                    if (check_default == null)
                    {
                        address.Default = 1;
                    }
                    else
                    {
                        address.Default = 0;
                    }
                    _context.Addresses.Add(address);
                    await _context.SaveChangesAsync();

                    Transport transport = new Transport();
                    transport.ShippingDate = null;
                    transport.Transporter = null;
                    transport.Status = 1;
                    transport.Type = cre_Ord.Type;
                    if (transport.Type == 1)
                    {
                        transport.Fee = 14000;
                    }
                    else
                    {
                        transport.Fee = 32000;
                    }
                    _context.Transports.Add(transport);
                    await _context.SaveChangesAsync();

                    Order order = new Order();
                    order.AddId = address.AddId;
                    order.CusId = cus_id_num;
                    order.TransId = transport.TransId;
                    order.DisId = null;
                    order.StoreId = cre_Ord.StoreId;
                    order.OrderDate = DateTime.Now;
                    order.SubTotal = cre_Ord.SubTotal;
                    order.OrderTotal = cre_Ord.OrderTotal;
                    order.PayBy = cre_Ord.PayBy;
                    if (cre_Ord.PayBy == 1)
                    {
                        order.Status = 1;
                    }
                    else
                    {
                        order.Status = 0;
                    }
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                //tạo khách hàng
                Customer customer = new Customer();
                customer.FirstName = cre_Ord.FirstName;
                customer.Gender = cre_Ord.Gender;
                customer.Phone = cre_Ord.Phone;
                customer.Email = null;
                customer.UserId = null;
                customer.RewardId = null;
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                //Tạo địa chỉ
                Address address = new Address();
                address.CusId = customer.CusId;
                address.SpecificAddress = cre_Ord.SpecificAddress;
                address.Ward = cre_Ord.Ward;
                address.District = cre_Ord.District;
                address.City = cre_Ord.City;
                address.Default = 1;
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                
                //tạo vận chuyển
                Transport transport = new Transport();
                transport.ShippingDate = null;
                transport.Transporter = null;
                transport.Status = 1;
                transport.Type = cre_Ord.Type;
                if (transport.Type == 1)
                {
                    transport.Fee = 14000;
                }
                else
                {
                    transport.Fee = 32000;
                }
                _context.Transports.Add(transport);
                await _context.SaveChangesAsync();

                //tạo hóa đơn
                Order order = new Order();
                order.AddId = address.AddId;
                order.CusId = customer.CusId;
                order.TransId = transport.TransId;
                order.DisId = null;
                order.StoreId = cre_Ord.StoreId;
                order.OrderDate = DateTime.Now;
                order.SubTotal = cre_Ord.SubTotal;
                order.OrderTotal = cre_Ord.OrderTotal;
                order.PayBy = cre_Ord.PayBy;
                if (cre_Ord.PayBy == 1)
                {
                    order.Status = 1;
                }
                else
                {
                    order.Status = 0;
                }
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
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
