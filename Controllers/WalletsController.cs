using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComplexProject;

namespace ComplexProject.Controllers
{
    public class WalletsController : Controller
    {
        readonly UnickDbContext _context;

        public WalletsController(UnickDbContext context)
        {
            _context = context;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            var unickDbContext = _context.Wallets.Include(w => w.IdUserNavigation);
            return View(await unickDbContext.ToListAsync());
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdWallet == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // GET: Wallets/Create
        public IActionResult Create()
        {
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser");
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdWallet,IdUser,Balance")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser", wallet.IdUser);
            return View("HomePage");
        }

        // GET: Wallets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser", wallet.IdUser);
            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdWallet,IdUser,Balance")] Wallet wallet)
        {
            if (id != wallet.IdWallet)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.IdWallet))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser", wallet.IdUser);
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdWallet == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wallets == null)
            {
                return Problem("Entity set 'UnickDbContext.Wallets'  is null.");
            }
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet != null)
            {
                _context.Wallets.Remove(wallet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalletExists(int id)
        {
          return (_context.Wallets?.Any(e => e.IdWallet == id)).GetValueOrDefault();
        }

    }
}
