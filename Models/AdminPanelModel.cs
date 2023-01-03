using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class AdminPanelModel : Controller
    {
        public User User { get; set; }
        public IQueryable<Auctionlot> AuctionLots { get; set; }
    }
}
