namespace WebBriliFresh.Models.DTO
{
    public class CreateOrderModel
    {
        //info for customer
        public string? FirstName { get; set; }

        public int? Gender { get; set; }

        public string? Phone { get; set; }

        //info for address
        public string? City { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

        public string? SpecificAddress { get; set; }


        //info for transport
        public int Type { get; set; }

        //info for order
        public int? StoreId { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal SubTotal { get; set; }
        public int PayBy { get; set; }
        public int Status { get; set; }
        public string ListOrder { get; set; }
    }
}
