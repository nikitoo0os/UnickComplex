using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class CategoryModel : Controller
    {
        public List<String> Categories { get; set; }

        public List<Auctionlot> Lots { get; set; }
    }
}
