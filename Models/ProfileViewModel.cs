using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class ProfileViewModel : Controller
    {
        public User User { get; set; }
        public Wallet Wallet { get; set; }
        public IQueryable<Auctionlot> AuctionLot { get; set; }
        public long IdProfile { get; set; }
    }
}
