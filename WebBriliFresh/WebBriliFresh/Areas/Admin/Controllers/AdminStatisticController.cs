using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminStatisticController : Controller
    {
        
        private readonly BriliFreshDbContext _context;
        public INotyfService _notifyService { get; }

        public AdminStatisticController(BriliFreshDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }

        public IActionResult Index(int? year)
        {
            ViewBag.TotalRevenue = RevenueStatistic(); // Thống kê tồng doanh thu
            ViewBag.TotalBenefit = BenefitStatistic(); // Thống kê tồng lợi nhuận
            ViewBag.TotalOrder = OrderStatistic(); // Thống kê tồng đơn hàng
            ViewBag.TotalCus = CusStatistic(); // Thống kê tồng khách hàng

            ViewBag.TotalMember = MemberStatistic(); // Thống kê tồng khách hàng thành viên
            ViewBag.TotalWalkInGuest = GuestStatistic(); // Thống kê tồng khách hàng vãng lai

            List<double> RevenueByMonth = new List<double>();
            List<double> BenefitByMonth = new List<double>();
            for (int i = 1; i <= 12; i++)
            {
                if (year == null)
                {
                    DateTime dt = DateTime.Now;
                    RevenueByMonth.Add(Convert.ToDouble(RevenueStatisticByMonth(i, dt.Year).ToString()));
                    BenefitByMonth.Add(Convert.ToDouble(BenefitStatisticByMonth(i, dt.Year).ToString()));
                }
                else
                {
                    RevenueByMonth.Add(Convert.ToDouble(RevenueStatisticByMonth(i, (int)year).ToString()));
                    BenefitByMonth.Add(Convert.ToDouble(BenefitStatisticByMonth(i, (int)year).ToString()));
                }
            }
            ViewBag.RevenueByMonth = RevenueByMonth;
            ViewBag.BenefitByMonth = BenefitByMonth;


            
            

            var topProducts = (from od in _context.OrderDetails.Include(od => od.Order).Include(od => od.Pro).Where(od => od.Order.Status == 1)
                     group od by new { od.ProId, od.Pro.ProName } into odp
                     select new
                     {
                         odp.Key.ProId, odp.Key.ProName,
                         Quantity = odp.Sum(q => q.Quantity)
                     }).OrderByDescending(i => i.Quantity).Take(5);
            List<int> topProId = new List<int>();
            List<string> topProName = new List<string>();
            List<int> topProSales = new List<int>();
            List<decimal> topProEarning = new List<decimal>();
            List<int> topProStockLeft = new List<int>();
            foreach (var item in topProducts)
            {
                topProId.Add(item.ProId);
                topProName.Add(item.ProName);
                topProSales.Add(item.Quantity);
                if (_context.Stocks.Where(s => s.ProId == item.ProId) == null)
                {
                    topProStockLeft.Add(0);
                }
                else
                {
                    topProStockLeft.Add(ProStockLeft(item.ProId));
                }
                if (_context.OrderDetails.Where(o => o.ProId == item.ProId) == null)
                {
                    topProEarning.Add(0);
                }
                else
                {
                    topProEarning.Add(ProEarning(item.ProId));
                }

            }
            ViewBag.topProId = topProId;
            ViewBag.topProName = topProName;
            ViewBag.topProSales = topProSales;
            ViewBag.topProEarning = topProEarning;
            ViewBag.topProStockLeft = topProStockLeft;

            DateTime d = DateTime.Now;
            List<int> yyyy = new List<int>();
            for (int i = d.Year; i>=2019; i--)
            {
                yyyy.Add(i);
            }
            ViewBag.year = yyyy;
            return View();
        }

        public decimal RevenueStatistic()
        {
            if (_context.Orders == null)
            {
                return 0;
            }
            else
            {
                decimal TotalRevenue = _context.Orders.Where(n => n.Status == 1).Sum(n => n.OrderTotal);
                return TotalRevenue;
            }
        }

        public decimal RevenueStatisticByMonth (int month, int year)
        {
            if (_context.Orders == null)
            {
                return 0;
            }
            else
            {
                var orderList = _context.Orders.Where(n => n.Status == 1 && n.OrderDate.Value.Month == month && n.OrderDate.Value.Year == year);
                decimal TotalRevenue = 0;
                foreach(var item in orderList){
                    TotalRevenue += decimal.Parse(item.OrderTotal.ToString());
                }
                return TotalRevenue;
            }
        }

        public decimal BenefitStatistic()
        {
            if (_context.Orders == null)
            {
                return 0;
            }
            else
            {
                //(_context.Orders.Include(n => n.Trans).Sum(n => n.OrderTotal - n.Trans.Fee).Value) -  (_context.OrderDetails.Include(od => od.Pro).Sum(od => od.Quantity * od.Pro.OriginalPrice).Value); - _context.OrderDetails.Include(od => od.Pro).Sum(od => od.Quantity * Decimal.ToDouble(od.Pro.OriginalPrice))));
                double thu = Decimal.ToDouble(_context.Orders.Where(n => n.Status == 1).Include(n => n.Trans).Sum(n => n.OrderTotal - n.Trans.Fee));

                double von = Decimal.ToDouble(_context.OrderDetails.Include(od => od.Order).Include(od => od.Pro).Where(od => od.Order.Status == 1).Sum(od => od.Quantity * od.Pro.OriginalPrice));
                
                decimal TotalBenefit = decimal.Parse((thu - von).ToString());
                return TotalBenefit;
            }
        }

        public decimal BenefitStatisticByMonth(int month, int year)
        {
            if (_context.Orders == null)
            {
                return 0;
            }
            else
            {
                //(_context.Orders.Include(n => n.Trans).Sum(n => n.OrderTotal - n.Trans.Fee).Value) -  (_context.OrderDetails.Include(od => od.Pro).Sum(od => od.Quantity * od.Pro.OriginalPrice).Value); - _context.OrderDetails.Include(od => od.Pro).Sum(od => od.Quantity * Decimal.ToDouble(od.Pro.OriginalPrice))));
                double thu = Decimal.ToDouble(_context.Orders.Where(n => n.Status == 1 && n.OrderDate.Value.Month == month && n.OrderDate.Value.Year == year).Include(n => n.Trans).Sum(n => n.OrderTotal - n.Trans.Fee));
               
                double von = Decimal.ToDouble(_context.OrderDetails.Include(od => od.Order).Include(od => od.Pro).Where(od => od.Order.Status == 1 && od.Order.OrderDate.Value.Month == month && od.Order.OrderDate.Value.Year == year).Sum(od => od.Quantity * od.Pro.OriginalPrice));
             
                decimal TotalBenefit = decimal.Parse((thu - von).ToString());
                return TotalBenefit;
            }
        }

        public double OrderStatistic()
        {
            double orderNum = _context.Orders.Count();
            return orderNum;
        }

        public double CusStatistic()
        {
            double cusNum = _context.Customers.Count();
            return cusNum;
        }

        public double MemberStatistic()
        {
            double memNum = _context.Customers.Where(c => c.UserId != null).Count();
            return memNum;
        }

        public double GuestStatistic()
        {
            double guestNum = _context.Customers.Where(c => c.UserId == null).Count();
            return guestNum;
        }

        public int ProStockLeft(int id)
        {
            int StockLeft = _context.Stocks.Where(s => s.ProId == id).Sum(s => (int)s.Quantity);
            return StockLeft;
        }
        public decimal ProEarning(int id)
        {
            decimal Earning = decimal.Parse(_context.OrderDetails.Where(o => o.ProId == id).Sum(o => o.Quantity * o.Price).ToString());
            return Earning;
        }
    }
}
