using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing.Printing;
using WebBriliFresh.Models;
namespace WebBriliFresh.Models.DAO
{
    public class ProductServices
    {

        private BriliFreshDbContext db = new BriliFreshDbContext();
        public Product GetProductByID(int ID)
        {
            return db.Products.Find(ID);
        }
        public Product checkNewProduct() {

            return (Product)db.Products.Where(x => (DateTime.Now - (DateTime)x.StartDate).TotalDays <= 3);
        }
        public Product checkDealProduct() {
            return (Product)db.Products.Where(x => db.DiscountProducts.Select(y => y.ProId).Contains(x.ProId));
        }
        public Product checkBriliProduct() {

            return (Product)db.Products.Where(x => x.Source.Equals("Sản phẩm của Brili"));
        }
        public Product checkImportProduct() {

            return (Product)db.Products.Where(x => x.Source.Equals("Hàng Nhập khẩu"));
        }



        public async Task<List<Product>> SearchProducts(string? search, int? minimumPrice, int? maximumPrice, int? typeID, int? storeID, List<string> selected, int? sortBy, int pageNo, int pageSize)
        {


            var products = await db.Products.ToListAsync();
            var store = db.Stocks.Where(x => x.StoreId == storeID);
            if (storeID.HasValue) {
                products = products.Where(x => store.Select(y => y.ProId).Contains(x.ProId)).ToList();
            }

            if (typeID.HasValue)
            {
                products = products.Where(x => x.TypeId == typeID.Value).ToList();
            }
            if (search != null && search != "0")
            {
                products = products.Where(x => x.ProName.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {

                products = products.ToList();

            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
            }
            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 1:
                        products = products.OrderBy(x => x.Price).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                }
            }
            //selectd {1 : deal soc ; 2: hang moi ; 3 : (source) hang Brili ; 4: (source) hang nhap khau}
            if (selected.Count() != 0)
            {

                for (int i = 0; i < selected.Count; i++) {
                    if (selected[i] == "1") {


                        products = products.Where(x => db.DiscountProducts.Where(z => z.Status == true).Select(y => y.ProId).Contains(x.ProId)).ToList();

                    }
                    if (selected[i] == "2") {
                        products = products.Where(x => (DateTime.Now - (DateTime)x.StartDate).TotalDays <= 3).ToList();

                    }
                    if (selected[i] == "3") {
                        products = products.Where(x => x.Source.Equals("Sản phẩm của Brili Fresh")).ToList();

                    }
                    if (selected[i] == "4") {
                        products = products.Where(x => x.Source.Equals("Sản phẩm nhập khẩu")).ToList();

                    }
                }

            }
            return products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        }

        public async Task<int> SearchProductsCount(string? search, int? minimumPrice, int? maximumPrice, int? typeID, int? storeID, List<string> selected, int? sortBy)
        {
            var products = await db.Products.ToListAsync();

            var store = db.Stocks.Where(x => x.StoreId == storeID);
            if (storeID.HasValue)
            {
                products = products.Where(x => store.Select(y => y.ProId).Contains(x.ProId)).ToList();
            }

            if (typeID.HasValue)
            {
                products = products.Where(x => x.TypeId == typeID.Value).ToList();
            }

            if (search != null && search != "0")
            {
                products = products.Where(x => x.ProName.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {

                products = products.ToList();

            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
            }
            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 1:
                        products = products.OrderBy(x => x.Price).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(x => x.Price).ToList();
                        break;
                }
            }
            //selectd {1 : deal soc ; 2: hang moi ; 3 : (source) hang Brili ; 4: (source) hang nhap khau}
            if (selected.Count() != 0)
            {

                for (int i = 0; i < selected.Count; i++)
                {
                    if (selected[i] == "1")
                    {
                        products = products.Where(x => db.DiscountProducts.Select(y => y.ProId).Contains(x.ProId)).ToList();

                    }
                    if (selected[i] == "2")
                    {
                        products = products.Where(x => (DateTime.Now - (DateTime)x.StartDate).TotalDays <= 3).ToList();


                    }
                    if (selected[i] == "3")
                    {
                        products = products.Where(x => x.Source.Equals("Sản phẩm của Brili Fresh")).ToList();

                    }
                    if (selected[i] == "4")
                    {
                        products = products.Where(x => x.Source.Equals("Sản phẩm nhập khẩu")).ToList();

                    }
                }

            }

            return products.Count();
        }

        public List<string> getImg(int? ProId) {
            var rs = db.Products.Where(x => x.ProId == ProId && x.IsDeleted == 0).FirstOrDefault();//.Include(x => x.ProductImages).Select(y => y.ProductImages.ImgData).ToList();
            var list_img = db.ProductImages.Where(x => x.ProId == rs.ProId).Select(x => x.ImgData).ToList();
            return list_img as List<string>;

        }

        public IEnumerable<Feedback> getFeedback(int? ProId) {
            var rs = db.Products.Where(x => x.ProId == ProId && x.IsDeleted == 0).FirstOrDefault();
            var fb = db.Feedbacks.Where(x => x.ProId == rs.ProId).ToList();
            return fb;

        }
        public List<string> getImgFB(int? fbID) {

            var fbImg = db.FeedbackImages.Where(x => x.FbId == fbID).Select(x => x.ImgData).ToList();

            return fbImg as List<string>;

        }
        public DiscountProduct getDiscount(int? ProId) {
            var rs = db.Products.Where(x => x.ProId == ProId && x.IsDeleted == 0).FirstOrDefault();
            var discount = db.DiscountProducts.Where(x => x.ProId == rs.ProId && x.Status == true).ToList();
            for (int i = 0; i < discount.Count; i++)
            {
                var nowdate = DateTime.Now;
                if (nowdate > (DateTime)discount[i].StartDate && nowdate < (DateTime)discount[i].EndDate)
                {
                    var discount_1 = new DiscountProduct();
                    discount_1 = discount[i];
                    return discount_1;
                }
            }

            var discount2 = new DiscountProduct();
            discount2.Value = 0;
            return discount2;
        }

        public Stock getStock(int? ProId, int? storeID) {


            var rs = db.Products.Where(x => x.ProId == ProId && x.IsDeleted == 0).FirstOrDefault();
            var stock = db.Stocks.Where(x => x.ProId == rs.ProId && x.StoreId == storeID).FirstOrDefault();
            return stock;
        }





        public bool SaveProduct(Product product)
        {
            db.Products.Add(product);
            return db.SaveChanges() > 0;
        }


        public class keyValue{
            public int proid;
            public int count;
        }

        public List<int> list_recomend(int proid)
        {
            List<int> OrderIdList = new List<int>();
            List<int> proIdList = new List<int>();
            List<int> proIdList_final = new List<int>();
            List<keyValue> listKeyValue = new List<keyValue>();

            OrderIdList = db.OrderDetails.Where(x => x.ProId == proid).Select(x => x.OrderId).ToList();
            for(int i =0;i< OrderIdList.Count; i++)
            {
                List<int> list_proid_a_order = db.OrderDetails.Where(x => x.OrderId == OrderIdList[i])
                                                              .Select(x => x.ProId).ToList();
                for (int j = 0; j < list_proid_a_order.Count; j++)
                {
                    proIdList.Add(list_proid_a_order[j]);
                }

            }
            bool[] checkid = new bool[proIdList.Count];

            for (int i = 0;i< proIdList.Count; i++)
            {
                bool x = true;
                for (int j = 0; j < i; j++)
                {
                    if (proIdList[i]== proIdList[j])
                    {
                        x = false; break;
                    }
                }
                checkid[i] = x;
            }

            for (int i = 0; i < proIdList.Count; i++)
            {
                if (checkid[i])
                {
                    int countid = 0;
                    for(int j=0;j< proIdList.Count; j++)
                    {
                        if (proIdList[j] == proIdList[i])
                        {
                            countid++;
                        }
                    }

                    var item = new keyValue { proid = proIdList[i],count = countid };
                    listKeyValue.Add(item);
                }
            }

            var temp = new keyValue();

            for (int i = 0; i < listKeyValue.Count; i++)
            {
                for(int j = i + 1; j < listKeyValue.Count; j++)
                {
                    if (listKeyValue[i].count< listKeyValue[j].count)
                    {
                        temp = listKeyValue[i];
                        listKeyValue[i] = listKeyValue[j];
                        listKeyValue[j] = temp;

                    }
                }
            }

            for (int i = 0; i < listKeyValue.Count; i++)
            {
                proIdList_final.Add(listKeyValue[i].proid);
            }

            return proIdList_final;
        }

      
    }
}

