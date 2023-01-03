using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ComplexProject.Controllers
{
    public class AuctionController : Controller
    {
        private readonly UnickDbContext _context;

        public AuctionController(UnickDbContext context)
        {
            _context = context;
        }

        [HttpPost] 
        public IActionResult Bids (int idLot)
        {
            var auctionBids = _context.Bids
                .FirstOrDefaultAsync(m => m.IdLot == idLot);

            return PartialView();
        }

    }
}
