using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class CreateAuctionlotModel : Controller
    {

        public HttpContext HttpContext { get; set; }
        public CreateAuctionlotModel(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }
        public CreateAuctionlotModel() { }

        public string Category { get; set; } = null!;

        public decimal StartPrice { get; set; }

        public decimal EndPrice { get; set; }

        public string Type { get; set; } = null!;

        public short PaybackTime { get; set; }

        public decimal AverageProfit { get; set; }

        public string Location { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string? Description { get; set   ; }

        public IFormFile Image { get; set; }


    }
    
    
}
