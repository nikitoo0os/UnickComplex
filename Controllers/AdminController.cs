using ComplexProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplexProject.Controllers
{
    public class AdminController : Controller
    {

        private readonly UnickDbContext _context;

        public AdminController(UnickDbContext context)
        {
            _context = context;
        }

        private User userSession { get; set; }
        public async Task<IActionResult> ChangeStatusToReady(int idLot)
        {
            var auctionLot = await _context.Auctionlots
                .FirstOrDefaultAsync(m => m.IdLot == idLot);

            var idUser = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            userSession = await _context.Users.FirstOrDefaultAsync(m => m.IdUser == idUser);

            auctionLot.Status = "Идёт аукцион";
            auctionLot.StartDate = DateOnly.FromDateTime(DateTime.Now);
            auctionLot.EndDate = auctionLot.StartDate.AddDays(7);

            await _context.SaveChangesAsync();

            return View("AdminPanel", new AdminPanelModel()
            {
                User = userSession,
                AuctionLots = _context.Auctionlots.Where(m => m.Status == "На проверке")
            });
        }
        public async Task<IActionResult> ChangeStatusToDiscard(int idLot)
        {
            var auctionLot = await _context.Auctionlots.FirstOrDefaultAsync(m => m.IdLot == idLot);

            auctionLot.Status = "Отказ";

            var idUser = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            userSession = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == idUser);

            await _context.SaveChangesAsync();

            return View("AdminPanel", new AdminPanelModel()
            {
                User = userSession,
                AuctionLots = _context.Auctionlots.Where(m => m.Status == "На проверке")
            });
        }
    }
}
