using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebBriliFresh.Common;
using WebBriliFresh.Models;
using WebBriliFresh.Models.DAO;
using WebBriliFresh.Models.ViewModels;

namespace WebBriliFresh.Controllers
{
    public class OverviewProductController : Controller
    {
<<<<<<< HEAD
        private BriliFreshDbContext db = new BriliFreshDbContext();
        ProductServices productService = new ProductServices();
=======
        //Tổng quan 3 loại danh mục
>>>>>>> origin/new_develop_3
        public IActionResult FishAndMeat()
        {
            return View();
        }

        //[Authorize(Policy = "LoggedIn")]
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

<<<<<<< HEAD
        [HttpGet]
        public async Task<ActionResult> Index(string? search, int? minimumPrice, int? maximumPrice, int? typeID, string? selecteString, int? sortBy, int? pageNo)
        {

            int pageSize = 16;
            ListProductsModel model = new ListProductsModel();

            model.TypeID = typeID.HasValue ? typeID.Value > 0 ? typeID.Value : 2 : 2;
            model.SortBy = sortBy.HasValue ? sortBy.Value > 0 ? sortBy.Value : 1 : 1;

            if (search == null)
            {
                search = "B";
            }
            
            model.searchTerm = search;
            if (selecteString == null)
            {
                selecteString = "0";
            }

            //List<int> defaultSelectOp = new List<int>();
            //defaultSelectOp.Add(2);
            //model.selectedOp = selected.Count != 0 ? selected : defaultSelectOp; 
            string[] temp = selecteString?.Trim().Split(",");
            List<string> addTemp = new List<string>();
            for (int i = 0; i < temp.Length; i++)
            {
                addTemp.Add(temp[i]);
            }
            model.selectedOp = addTemp;
            model.MaximumPrice = maximumPrice.HasValue ? maximumPrice.Value > 0 ? maximumPrice.Value : ((int)db.Products.Max(x => x.Price)) : ((int)db.Products.Max(x => x.Price));
            model.MinPrice = minimumPrice.HasValue ? minimumPrice.Value > 0 ? minimumPrice.Value : 0 : 0;
            //model.MaximumPrice = (int)db.Products.Max(x => x.Price);
            model.InitialMaximumPrice = (int)db.Products.Max(x => x.Price);


            // model.Sizes = db.Sizes.ToList();

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            int totalCount = await productService.SearchProductsCount(model.searchTerm, model.MinPrice, model.MaximumPrice, model.TypeID, model.selectedOp, model.SortBy);
            model.Products = await productService.SearchProducts(model.searchTerm, model.MinPrice, model.MaximumPrice, model.TypeID, model.selectedOp, model.SortBy, (int)pageNo, pageSize);

            model.Pager = new Pager(totalCount, (int)pageNo, pageSize);
            return View(model);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> FilterProducts(string? search,int? minimumPrice, int? maximumPrice, int? typeID, string? selecteString, int? sortBy, int? pageNo, int? pageSize)
        {

            FilterViewModel model = new FilterViewModel();
            model.TypeID = typeID;
            model.SortBy = sortBy;
            if (search == null)
            {
                search = "B";
            }
            else
            {
                search = search;
            }
            model.searchTerm = search;

            List<string> addTemp = new List<string>();


            if (selecteString == null)
            {
                selecteString = "0";
            }
            else
            {
                selecteString = selecteString;
            }

            string[]? temp = selecteString?.Trim().Split(',');
            if (temp.Length != null)
            {
                for (int i = 0; i < temp?.Length; i++)
                {
                    addTemp.Add(temp[i]);
                }
            }
            else
            {
                addTemp.Add("0");
            }
            model.selectedOp = addTemp;
            //model.MaximumPrice = maximumPrice.HasValue ? maximumPrice.Value > 0 ? maximumPrice.Value : ((int)db.Products.Max(x => x.Price)) : ((int)db.Products.Max(x => x.Price));
            //model.MinPrice = minimumPrice.HasValue ? minimumPrice.Value > 0 ? minimumPrice.Value : 0 : 0;


            model.MaximumPrice = (int)maximumPrice.Value;
            model.MinPrice = (int)minimumPrice.Value;
            //model.MaximumPrice = (int)db.Products.Max(x => x.Price);
            model.InitialMaximumPrice = (int)db.Products.Max(x => x.Price);
            pageSize = pageSize.HasValue ? pageSize.Value > 0 ? pageSize.Value : 16 : 16;
            model.PageSize = pageSize;


            // model.Sizes = db.Sizes.ToList();

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            int totalCount = await productService.SearchProductsCount(model.searchTerm,minimumPrice, maximumPrice, typeID, model.selectedOp, sortBy);
            model.Products = await productService.SearchProducts(model.searchTerm,minimumPrice, maximumPrice, typeID, model.selectedOp, sortBy, (int)pageNo, (int)pageSize);

            model.Pager = new Pager(totalCount, (int)pageNo, (int)pageSize);
            return PartialView(model);
        }
=======
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

>>>>>>> origin/new_develop_3

    }
}
