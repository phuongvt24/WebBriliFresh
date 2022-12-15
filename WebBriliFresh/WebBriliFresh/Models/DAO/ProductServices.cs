using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace WebBriliFresh.Models.DAO
{
    public class ProductServices
    {

        private BriliFreshDbContext db = new BriliFreshDbContext();
        public Product GetProductByID(int ID)
        {
            return db.Products.Find(ID);
        }
        public Product checkNewProduct(){

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



        public async Task<List<Product>> SearchProducts(string? search,int? minimumPrice, int? maximumPrice, int? typeID, List<string> selected, int? sortBy, int pageNo, int pageSize)
        {


            var products = await db.Products.ToListAsync();

           
            if (typeID.HasValue)
            {
                products = products.Where(x => x.TypeId == typeID.Value).ToList();
            }
            if (search != null)
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
            if (selected.Count()!=0)
            {

                for (int i = 0; i < selected.Count; i++) {
                    if (selected[i] == "1") {
                        products = products.Where(x => (DateTime.Now - (DateTime)x.StartDate).TotalDays <= 3).ToList();

                    }
                    if (selected[i] == "2") {
                        products = products.Where(x => db.DiscountProducts.Select(y => y.ProId).Contains(x.ProId)).ToList();

                    }
                    if (selected[i] == "3") {
                        products = products.Where(x => x.Source.Equals("Sản phẩm của Brili")).ToList();

                    }
                    if (selected[i] == "4") {
                        products = products.Where(x => x.Source.Equals("Hàng Nhập khẩu")).ToList();

                    }
                }

            }
            return products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        }

        public async Task<int> SearchProductsCount(string? search, int? minimumPrice, int? maximumPrice, int? typeID, List<string> selected, int? sortBy)
        {
            var products = await db.Products.ToListAsync();
           

            if (typeID.HasValue)
            {
                products = products.Where(x => x.TypeId == typeID.Value).ToList();
            }
            if (search != null)
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
                        products = products.Where(x => (DateTime.Now - (DateTime)x.StartDate).TotalDays <= 3).ToList();

                    }
                    if (selected[i] == "2")
                    {
                        products = products.Where(x => db.DiscountProducts.Select(y => y.ProId).Contains(x.ProId)).ToList();

                    }
                    if (selected[i] == "3")
                    {
                        products = products.Where(x => x.Source.Equals("Sản phẩm của Brili Fresh")).ToList();

                    }
                    if (selected[i] == "4")
                    {
                        products = products.Where(x => x.Source.Equals("Hàng Nhập khẩu")).ToList();

                    }
                }

            }

            return products.Count();
        }

        public string getImg(int? id) {
            string rs = db.ProductImages.Where(x => x.ProId == id).Select(y => y.ImgData).ToString();
            return rs;

        }

        public bool SaveProduct(Product product)
        {
            db.Products.Add(product);
            return db.SaveChanges() > 0;
        }

      
    }
}

