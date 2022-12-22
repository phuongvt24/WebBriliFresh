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
using Stripe.Checkout;

namespace WebBriliFresh.Controllers
{
    public class BuyAndPayController : Controller
    {
        private readonly BriliFreshDbContext _context;
        public INotyfService _notifyService { get; }

        public BuyAndPayController(BriliFreshDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
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


        public IActionResult AddToCart(int proId, int storeid, decimal saleprice, int? quantity, string type = "normal")
        {
            var myCart = Carts;
            var item = myCart.Where(z => z.StoreId == storeid).Where(p => p.ProductId == proId).SingleOrDefault();
            if (quantity != null)
            {
                if (item == null)
                {
                    item = new ShoppingCartViewModel
                    {
                        ProductId = proId,
                        Quantity = (int)quantity,
                        StoreId = storeid,
                        SalePrice = saleprice
                    };
                    myCart.Add(item);
                }
                else
                {
                    item.Quantity = (int)quantity;
                }
            }
            else
            {


                if (item == null)
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
            }
            HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
            if (type == "ajax")
            {

                return Json(new
                {
                    quantity = Carts.Where(x => x.StoreId == storeid).Sum(p => p.Quantity)
                });
            }
            return RedirectToAction("ListFishAndMeat", "OverviewProduct");
        }

        public IActionResult Delete(int proId, int storeid)
        {
            var myCart = Carts;
            if (Carts != null)
            {
                ShoppingCartViewModel model = myCart.Where(x => x.StoreId == storeid).Where(p => p.ProductId == proId).FirstOrDefault();
                if (model != null)
                {
                    myCart.Remove(model);
                }
                HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
                return Json(new
                {
                    quantity = Carts.Where(x => x.StoreId == storeid).Sum(p => p.Quantity)
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public IActionResult Update(int proId, int quantity, int storeid)
        {
            var myCart = Carts;
            var item = myCart.Where(p => p.ProductId == proId).Where(x => x.StoreId == storeid).FirstOrDefault();
            item.Quantity = quantity;
            HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
            return Json(new
            {
                quantity = Carts.Where(x => x.StoreId == storeid).Sum(p => p.Quantity)
            });
        }


        //[HttpPost]
        //public JsonResult checkout(string checkoutitem )
        //{
        //    var model2 = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(checkoutitem);
        //    return Json(model2);
        //}



        public async Task<IActionResult> Create([Bind("FirstName,Gender,Phone,City,District,Ward,SpecificAddress,Type,StoreId,OrderTotal,SubTotal,PayBy,Status,ListOrder,CusId,AddressId,DisId")] CreateOrderModel cre_Ord)
        {
            var check_address_1 = _context.Addresses.Where(x => x.CusId == cre_Ord.CusId).ToList();
            var order_details = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cre_Ord.ListOrder);
            var domain = "https://localhost:44307/";

            Transport transport = new Transport();
            Order order = new Order();


            if (cre_Ord.CusId > 0)
            {
                if (cre_Ord.AddressId > 0)
                {
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

                    order.AddId = cre_Ord.AddressId;
                    order.CusId = cre_Ord.CusId;
                    order.TransId = transport.TransId;
                    if (cre_Ord.DisId > 0)
                    {
                        order.DisId = cre_Ord.DisId;
                    }
                    else
                    {
                        order.DisId = null;
                    }

                    order.StoreId = cre_Ord.StoreId;
                    order.OrderDate = DateTime.Now;
                    order.SubTotal = cre_Ord.SubTotal;
                    order.OrderTotal = cre_Ord.OrderTotal;
                    order.PayBy = cre_Ord.PayBy;
                    if (cre_Ord.PayBy == 1)
                    {
                        order.Status = 0;
                    }
                    else
                    {
                        order.Status = 1;
                    }
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < order_details.Count; i++)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = order.OrderId;
                        orderDetail.ProId = order_details[i].ProductId;
                        orderDetail.Quantity = order_details[i].Quantity;
                        orderDetail.Price = order_details[i].SalePrice;
                        _context.OrderDetails.Add(orderDetail);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    Address address = new Address();
                    address.CusId = cre_Ord.CusId;
                    address.SpecificAddress = cre_Ord.SpecificAddress;
                    address.Ward = cre_Ord.Ward;
                    address.District = cre_Ord.District;
                    address.City = cre_Ord.City;
                    if (check_address_1.Count == 0)
                    {
                        address.Default = 1;
                    }
                    else
                    {
                        address.Default = 0;
                    }

                    _context.Addresses.Add(address);

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

                    order.AddId = address.AddId;
                    order.CusId = cre_Ord.CusId;
                    order.TransId = transport.TransId;
                    if (cre_Ord.DisId > 0)
                    {
                        order.DisId = cre_Ord.DisId;
                    }
                    else
                    {
                        order.DisId = null;
                    }
                    order.StoreId = cre_Ord.StoreId;
                    order.OrderDate = DateTime.Now;
                    order.SubTotal = cre_Ord.SubTotal;
                    order.OrderTotal = cre_Ord.OrderTotal;
                    order.PayBy = cre_Ord.PayBy;
                    if (cre_Ord.PayBy == 1)
                    {
                        order.Status = 0;
                    }
                    else
                    {
                        order.Status = 1;
                    }
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < order_details.Count; i++)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = order.OrderId;
                        orderDetail.ProId = order_details[i].ProductId;
                        orderDetail.Quantity = order_details[i].Quantity;
                        orderDetail.Price = order_details[i].SalePrice;
                        _context.OrderDetails.Add(orderDetail);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            //không đăng nhập tài khoản
            else
            {
                var cus_id_num = _context.Customers.Where(x => x.Phone == cre_Ord.Phone).Select(x => x.CusId).FirstOrDefault();
                if (cus_id_num > 0)
                {
                    var check_address = _context.Addresses.Where(x => x.SpecificAddress == cre_Ord.SpecificAddress)
                                                          .Where(z => z.Ward == cre_Ord.Ward)
                                                          .Where(z => z.District == cre_Ord.District)
                                                          .Where(z => z.City == cre_Ord.City)
                                                          .Select(a => a.AddId)
                                                          .FirstOrDefault();
                    if (check_address > 0)
                    {
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

                        order.AddId = check_address;
                        order.CusId = cus_id_num;
                        order.TransId = transport.TransId;
                        order.DisId = null;
                        order.StoreId = cre_Ord.StoreId;
                        order.OrderDate = DateTime.Now;
                        order.SubTotal = cre_Ord.SubTotal;
                        order.OrderTotal = cre_Ord.OrderTotal;
                        order.PayBy = cre_Ord.PayBy;
                        order.Status = 0;
                        order.Status = 1;

                        _context.Orders.Add(order);
                        await _context.SaveChangesAsync();

                        for (int i = 0; i < order_details.Count; i++)
                        {
                            OrderDetail orderDetail = new OrderDetail();
                            orderDetail.OrderId = order.OrderId;
                            orderDetail.ProId = order_details[i].ProductId;
                            orderDetail.Quantity = order_details[i].Quantity;
                            orderDetail.Price = order_details[i].SalePrice;
                            _context.OrderDetails.Add(orderDetail);
                            await _context.SaveChangesAsync();
                        }


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

                        order.AddId = address.AddId;
                        order.CusId = cus_id_num;
                        order.TransId = transport.TransId;
                        order.DisId = null;
                        order.StoreId = cre_Ord.StoreId;
                        order.OrderDate = DateTime.Now;
                        order.SubTotal = cre_Ord.SubTotal;
                        order.OrderTotal = cre_Ord.OrderTotal;
                        order.PayBy = cre_Ord.PayBy;
                        order.Status = 0;


                        _context.Orders.Add(order);
                        await _context.SaveChangesAsync();

                        for (int i = 0; i < order_details.Count; i++)
                        {
                            OrderDetail orderDetail = new OrderDetail();
                            orderDetail.OrderId = order.OrderId;
                            orderDetail.ProId = order_details[i].ProductId;
                            orderDetail.Quantity = order_details[i].Quantity;
                            orderDetail.Price = order_details[i].SalePrice;
                            _context.OrderDetails.Add(orderDetail);
                            await _context.SaveChangesAsync();
                        }
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
                    order.AddId = address.AddId;
                    order.CusId = customer.CusId;
                    order.TransId = transport.TransId;
                    order.DisId = null;
                    order.StoreId = cre_Ord.StoreId;
                    order.OrderDate = DateTime.Now;
                    order.SubTotal = cre_Ord.SubTotal;
                    order.OrderTotal = cre_Ord.OrderTotal;
                    order.Status = 0;
                    order.PayBy = cre_Ord.PayBy;
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < order_details.Count; i++)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = order.OrderId;
                        orderDetail.ProId = order_details[i].ProductId;
                        orderDetail.Quantity = order_details[i].Quantity;
                        orderDetail.Price = order_details[i].SalePrice;
                        _context.OrderDetails.Add(orderDetail);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            if (cre_Ord.DisId > 0)
            {
                var discount = _context.DiscountOrders.Where(x => x.DisId == cre_Ord.DisId).FirstOrDefault();
                discount.Status = true;
                _context.DiscountOrders.Update(discount);
                await _context.SaveChangesAsync();
            }

            for (int i = 0; i < order_details.Count; i++)
            {
                Stock detail = new Stock();
                detail = _context.Stocks.Where(x => x.ProId == order_details[i].ProductId).Where(c => c.StoreId == cre_Ord.StoreId).FirstOrDefault();
                detail.Quantity -= order_details[i].Quantity;
                _context.Stocks.Update(detail);
                await _context.SaveChangesAsync();
            };

            if (cre_Ord.PayBy == 7)
            {
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                    {
                    },
                    Mode = "payment",
                    SuccessUrl = domain + $"BuyAndPay/SuccessPayment?id={order.OrderId}",
                    CancelUrl = domain + $"BuyAndPay/Cancel",
                };
                foreach (var item in order_details)
                {
                    string itemName = _context.Products.FirstOrDefault(a => a.ProId == item.ProductId).ProName;
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long?)(item.SalePrice),
                            Currency = "VND",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = itemName,
                            },
                        },
                        Quantity = item.Quantity,
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);

                Response.Headers.Add("location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction(nameof(SuccessPayment));
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

        public void ClearCard(int? orderId)
        {
            var myCart = Carts;
            var order = _context.Orders.Include(a => a.OrderDetails).FirstOrDefault(a => a.OrderId == orderId);
            if (order != null)
            {
                order.Status = 1;
                var order_details = order.OrderDetails.ToList();

                if (Carts != null)
                {
                    for (int i = 0; i < order_details.Count; i++)
                    {
                        var item = myCart.Where(p => p.ProductId == order_details[i].ProId).Where(x => x.StoreId == order.StoreId).FirstOrDefault();
                        if (item != null)
                            myCart.Remove(item);
                    };
                    HttpContext.Session.Set(CommonConstants.SessionCart, myCart);
                }
            }
        }

        [HttpGet]
        public IActionResult SuccessPayment(int? id)
        {
            ClearCard(id);
         
            return View();
        }
        [HttpGet]
        public IActionResult Cancel()
        {
            return View();
        }
        public async Task<IActionResult> DeliveryInfoLogin()
        {
            var cusid = HttpContext.Session.GetInt32("CUS_SESSION_CUSID");
            var address = _context.Addresses.Include(x => x.Cus).Where(x => x.CusId == cusid);
            return View(await address.ToListAsync());
        }

        public IActionResult PayInfo()
        {

            return View();
        }


    }
}
