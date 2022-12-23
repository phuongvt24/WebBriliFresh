

namespace WebBriliFresh.Models.ViewModels
{
    public class DetailsProduct
    {

        public Product product {get; set;}
        public List<string> productImgs { get; set; }

        public List<Feedback> feedbacks { get; set; }


        public DiscountProduct discount { get; set; }

        public Stock stock { get; set; }
    }
}
