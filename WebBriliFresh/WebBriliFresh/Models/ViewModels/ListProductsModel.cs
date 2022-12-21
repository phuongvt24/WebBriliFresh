using WebBriliFresh.Common;

namespace WebBriliFresh.Models.ViewModels
{
    public class ListProductsModel
    {
        public List<Product> Products { get; set; }
        public string searchTerm { get; set; }
        public int MaximumPrice { get; set; }
        public int MinPrice { get; set; }
        public int? SortBy { get; set; }
        public int? TypeID { get; set; }
        public int? StoreID { get; set; }   
        public List<string> selectedOp { get; set; }
        public Pager Pager { get; set; }
        public int InitialMaximumPrice { get; set; }

    }
    public class FilterViewModel {
        public List<Product> Products { get; set; }

        public string searchTerm { get; set; }

        public int MaximumPrice { get; set; }
        public int MinPrice { get; set; }
        public int? SortBy { get; set; }
        public int? TypeID { get; set; }
        public int? StoreID { get; set; }


        public List<string> selectedOp { get; set; }
        public Pager Pager { get; set; }
        public int? PageSize { get; set; }
        public int InitialMaximumPrice { get; set; }
    }
}
