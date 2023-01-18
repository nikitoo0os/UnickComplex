using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComplexProject;
using ComplexProject.Models;

namespace ComplexProject.Controllers
{
    public class WalletController : Controller, IController
    {
        readonly UnickDbContext _context;

        public WalletController(UnickDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> get(int? id)
        {
            int temp = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            if (temp == id)
            {
                var wallet = _context.Wallets
                    .Where(m => m.IdUser == id)
                    .ToList();

                var transactions = _context.Transactions
                    .Where(m => m.IdSender == id || m.IdReceiver == id)
                    .ToList();

                return View("wallet", new WalletModel()
                {
                    Wallet = wallet,
                    Transactions = transactions
                });
            }

            return Redirect("HomePage");
        }
    }
}
