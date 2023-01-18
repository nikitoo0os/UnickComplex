using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ComplexProject.Models
{
    public class ProfileViewModel : Controller
    {
        public User User { get; set; }
        public Wallet Wallet { get; set; }
        public IQueryable<Auctionlot> AuctionLot { get; set; }
        public long IdProfile { get; set; }

        [BindingBehavior(BindingBehavior.Optional)]
        public ImageModel ImageModel { get; set; }
    }
}
