namespace WebBriliFresh.Models.DTO
{
    public class ShoppingCartViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int StoreId { get; set; }
        public decimal SalePrice { get; set; }
    }
}
