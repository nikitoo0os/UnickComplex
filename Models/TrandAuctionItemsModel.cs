using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class TrandAuctionItemsModel : Controller
    {
        public List<Auctionlot> TrandAuctionLots { get; set; }
    }
}
