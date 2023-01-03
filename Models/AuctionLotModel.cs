using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ComplexProject.Models
{
    public class AuctionLotModel : Controller
    {
        public Auctionlot AuctionLot { get; set; }
        public Wallet Wallet { get; set; }
        public Bid LastBid { get; set; }
        public long IdProfile { get; set; }

    }
}
