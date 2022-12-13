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

        public IActionResult Index()
        {
            ViewBag.TotalRevenue = RevenueStatistic(); // Thống kê tồng doanh thu
            ViewBag.TotalBenefit = BenefitStatistic(); // Thống kê tồng lợi nhuận
            ViewBag.TotalOrder = OrderStatistic(); // Thống kê tồng đơn hàng
            ViewBag.TotalCus = CusStatistic(); // Thống kê tồng khách hàng

            ViewBag.TotalMember = MemberStatistic(); // Thống kê tồng khách hàng thành viên
            ViewBag.TotalWalkInGuest = GuestStatistic(); // Thống kê tồng khách hàng vãng lai
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
                decimal TotalRevenue = _context.Orders.Sum(n => n.OrderTotal);
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
                var orderList = _context.Orders.Where(n => n.OrderDate.Value.Month == month && n.OrderDate.Value.Year == year);
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
                double thu = Decimal.ToDouble(_context.Orders.Include(n => n.Trans).Sum(n => n.OrderTotal - n.Trans.Fee));
                double von = Decimal.ToDouble(_context.OrderDetails.Include(od => od.Pro).Sum(od => od.Quantity * od.Pro.OriginalPrice));
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
    }
}
