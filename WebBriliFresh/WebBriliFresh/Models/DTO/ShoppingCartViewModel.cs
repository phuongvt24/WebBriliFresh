namespace WebBriliFresh.Models.DTO
{
    [Serializable]
    public class ShoppingCartViewModel
    {
        public int quantity { get; set; }
        public int productId { get; set; }
        public int storeId { get; set; }
        public decimal saleprice { get; set; }
    }
}
