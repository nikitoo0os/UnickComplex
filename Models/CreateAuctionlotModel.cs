using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;

namespace ComplexProject.Models
{
    public class CreateAuctionlotModel : Controller
    {
        public string Category { get; set; } = null!;

        public decimal StartPrice { get; set; }

        public decimal EndPrice { get; set; }

        public string Type { get; set; } = null!;

        public short PaybackTime { get; set; }

        public decimal AverageProfit { get; set; }

        public string Location { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string? Description { get; set   ; }

        public string ImageURL { get; set; }

        [Display(Name = "Картинка болга")]
        [BindingBehavior(BindingBehavior.Optional)]
        public ImageModel ImageModel { get; set; }
    }
    
    
}
