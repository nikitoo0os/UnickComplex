using ComplexProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using NuGet.Protocol.Core.Types;
using System;
using System.IO.Pipelines;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ComplexProject.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Create
        private readonly UnickDbContext _context;

        public AccountController(UnickDbContext context)
        {
            _context = context;
            
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }


        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUser,IdWallet,IdSupportProject,IdProject,Login,Password,FirstName,SecondName,DateOfBirth,DateOfRegistration")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = sha256_hash(user.Password);
                try
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return View("AuthPageErr");
                }

                return View("HomePage");
            }
            return View("HomePage");

        }

        public async Task<IActionResult> CreateWallet([Bind("IdWallet,IdUser,Balance")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(wallet);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return View("AuthPageErr");
                }

                return View("HomePage");
            }
            return View("HomePage");

        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdUser,IdWallet,IdSupportProject,IdProject,Login,Password,FirstName,SecondName,DateOfBirth,DateOfRegistration")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.IdUser))
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
            return View("Profile");
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'UnickDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.IdUser == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("IdUser,IdWallet,IdSupportProject,IdProject,Login,Password,FirstName,SecondName,DateOfBirth,DateOfRegistration")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = sha256_hash(user.Password);
                DateOnly now = DateOnly.Parse(DateTime.Now.ToString("d"));
                user.DateOfRegistration = now;
                Wallet wallet = new Wallet()
                {
                    IdWallet = user.IdUser
                };
                user.Wallets.Add(wallet);

                try
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    CreateCookie("Unick_User_ID", user.IdUser.ToString());
                    CreateCookie("Unick_User_Token", user.Password);
                    CreateCookie("Unick_Account_Access", "True");

                    var auctionLots = _context.Auctionlots.Where(m => m.IdAuctioneer == user.IdUser);

                    return View("Profile", new ProfileViewModel()
                    {
                        User = user,
                        Wallet = wallet,
                        AuctionLot = auctionLots
                    });

                }
                catch
                {
                    return View("AuthPageErr");
                }


                //return View("HomePage");
            }
            return View("HomePage");
        }
        private User userSession;
        public async Task<IActionResult> LoginWithCookies()
        {
            int id = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);
            var token = HttpContext.Request.Cookies["Unick_User_Token"];
            var access = HttpContext.Request.Cookies["Unick_Auth_User"];

            if (id != 0 | access == "True")
            {
                ProfileViewModel pvm = new ProfileViewModel();
                userSession = await _context.Users
                    .FirstOrDefaultAsync(m => m.IdUser == id);

                if (userSession.Role == "admin")
                {
                    return View("AdminPanel", new AdminPanelModel()
                    {
                        User = userSession,
                        AuctionLots = _context.Auctionlots.Where(m => m.Status == "На проверке")
                    });
                }

                var wallet1 = await _context.Wallets
                    .FirstOrDefaultAsync(m => m.IdUser == id);

                //var auctionslots = await _context.Auctionlots
                //    .FirstOrDefaultAsync(m => m.IdAuctioneer == id);

                var auctionLots = _context.Auctionlots.Where(m => m.IdAuctioneer == id);

                if (userSession != null && wallet1 != null)
                {
                    pvm = new ProfileViewModel()
                    {
                        User = userSession,
                        Wallet = wallet1,
                        AuctionLot = auctionLots,
                        IdProfile = userSession.IdUser
                    };
                }


                return View("Profile", pvm);

            }
            return View("LoginPage");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Login,Password")] LoginViewModel model)
        {

            if (model.Login != null && model.Password != null)
            {
                userSession = await _context.Users
               .FirstOrDefaultAsync(m => m.Login == model.Login);

                if (userSession.Role == "admin")
                {
                    return View("AdminPanel", new AdminPanelModel()
                    {
                        User = userSession,
                        AuctionLots = _context.Auctionlots.Where(m => m.Status == "На проверке")
                    });
                }

                var wallet1 = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.IdUser == userSession.IdUser);

                var auctionLots = _context.Auctionlots.Where(m => m.IdAuctioneer == userSession.IdUser);

                if (userSession != null && sha256_hash(model.Password) == userSession.Password)
                {
                    DeleteCookie("Unick_User_ID");
                    DeleteCookie("Unick_Auth_User");

                    CreateCookie("Unick_User_ID", userSession.IdUser.ToString());
                    CreateCookie("Unick_Auth_User", "True");

                    return View("Profile", new ProfileViewModel()
                    {
                        User = userSession,
                        Wallet = wallet1,
                        AuctionLot = auctionLots,
                        IdProfile = userSession.IdUser
                    });
                }
            }


            return View("AuthPageErr");
        }

        public async Task<IActionResult> Logout()
        {
            DeleteCookie("Unick_User_ID");
            DeleteCookie("Unick_Auth_User");

            return Redirect("/");
        }

        public IActionResult EditAccountProfile()
        {
            var user = _context.Users
               .FirstOrDefault(m => m.IdUser == Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]));
            return View("EditProfile", user);
        }

        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));
                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }

        public IActionResult CreateCookie(string key, string value)
        {
            CookieOptions cookieOptions = new();
            cookieOptions.Expires = DateTime.Now.AddDays(15);
            Response.Cookies.Append(key, value, cookieOptions);

            return View("HomePage");
        }
        public void DeleteCookie(string key)
        {
            Response.Cookies.Delete(key);
        }

        public IActionResult CreateAuctionItem()
        {
            return View("CreateAuctionItem");
        }



        [HttpPost]
        public async Task<IActionResult> CreateAuctionLot([Bind] CreateAuctionlotModel createModel)
        {
            Auctionlot model = new Auctionlot();
            model.Title = createModel.Title;
            model.Description = createModel.Description;
            model.AverageProfit = createModel.AverageProfit;
            model.Category = createModel.Category;
            model.StartPrice = createModel.StartPrice;
            model.EndPrice = createModel.EndPrice;
            model.Type= createModel.Type;
            model.Location = createModel.Location;
                

            model.IdAuctioneer = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);
            model.AttachmentsLink = new string[]
            {
                "eflpskodgrjtf"
            };
            model.Status = "На проверке";
            model.Winner = "";

            var image = createModel.Image;
            if(image != null)
            {
               
                var filePath = Path.GetTempFileName();


                if (image.Length > 0)
                    {
                        using (var stream = new FileStream("~/wwwroot/img", FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                    }
               
            }


            model.ImageLink = new string[]
            {
                "jrigthjfk"
            };

            var user = _context.Users
               .FirstOrDefault(m => m.IdUser == Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]));
            var wallet = _context.Wallets
                .FirstOrDefault(m => m.IdUser == user.IdUser);
            var auctionLots = _context.Auctionlots.Where(m => m.IdAuctioneer == user.IdUser);




            user.Auctionlots.Add(model);
            await CreateTag(model.Category);

            if (!ModelState.IsValid)
            {                

                await _context.SaveChangesAsync();

                var curLot = _context.Auctionlots
                    .LastOrDefault(m => m.IdAuctioneer == user.IdUser);

                var curTag = await _context.Tags
                    .FirstOrDefaultAsync(m => m.Name == model.Category);

                curLot.Auctiontags.Add(new Auctiontag()
                {
                    IdLot = curLot.IdLot,
                    IdTag = curTag.IdTag
                });

                await CreateActivity("Создание лота", curLot.IdLot, user.IdUser);

                await _context.SaveChangesAsync();

                return View("Profile", new ProfileViewModel()
                {
                    User = user,
                    Wallet = wallet,
                    AuctionLot = auctionLots,
                    IdProfile = user.IdUser
                });
            }

            return Redirect("/");
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        private async Task CreateTag(string name)
        {
            var currTag = await _context.Tags.FirstOrDefaultAsync(m => m.Name == name);

            if (currTag == null) {
                _context.Tags.Add(new Tag(name));
            }
        }
        private async Task CreateActivity(string type, long idLot, int idUser)
        {
            await _context.Activities.AddAsync(new Activity()
            {
                IdLot = idLot,
                IdUser = idUser,
                Type = type
            });
        }

        public async Task<IActionResult> GetAuctionItem(long id) {

            var idUser = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            var auctionLot = await _context.Auctionlots
                .FirstOrDefaultAsync(m => m.IdLot == id);

            var userWallet = await _context.Wallets
                .FirstOrDefaultAsync(m => m.IdUser == idUser);

            var lotBids = _context.Bids
                .Where(m => m.IdLot == id);


            if (lotBids.Any())
            {
                Bid lastBids = lotBids
                                .OrderBy(m => m.Price)
                                .Last();

                var currentDate = DateOnly.FromDateTime(DateTime.Now);

                if (auctionLot.EndDate.DayNumber - currentDate.DayNumber <= 0 && auctionLot.Status != "Завершено")
                {
                    auctionLot.Winner = lastBids.IdUser.ToString();
                    auctionLot.EndPrice = lastBids.Price;
                    auctionLot.Status = "Завершено";

                    await TransferMoney(auctionLot.EndPrice, auctionLot.Winner, auctionLot.IdAuctioneer);
                    
                    _context.SaveChanges();
                }

                return View("AuctionItem", new AuctionLotModel()
                {
                    AuctionLot = auctionLot,
                    Wallet = userWallet,
                    LastBid = lastBids,
                    IdProfile = idUser
                    
                });
            }
            else
            {
                return View("AuctionItem", new AuctionLotModel()
                {
                    AuctionLot = auctionLot,
                    Wallet = userWallet,
                    IdProfile = idUser
                });
            }



        }

        public async Task<IActionResult> AdminGetAuctionItem(long idLot)
        {

            var idUser = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            var auctionLot = await _context.Auctionlots
                .FirstOrDefaultAsync(m => m.IdLot == idLot);

            var userWallet = await _context.Wallets
                .FirstOrDefaultAsync(m => m.IdUser == idUser);


            return View("AdminPanelAuctionItem", new AuctionLotModel()
            {
                AuctionLot = auctionLot,
                Wallet = userWallet
            });

        }

        public async Task<IActionResult> GetUserProfile(long idUser)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == idUser);

            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(m => idUser == idUser);

            var auctionLots = _context.Auctionlots
                .Where(m => m.IdAuctioneer == idUser);

            var idProfile = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            return View("Profile", new ProfileViewModel()
            {
                User = user,
                Wallet = wallet,
                AuctionLot = auctionLots,
                IdProfile = idProfile
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bid(int idUser, long idLot, decimal amount)
        {

            var auctionLot = await _context.Auctionlots
                .FirstOrDefaultAsync(m => m.IdLot == idLot);

            var userWallet = await _context.Wallets
                .FirstOrDefaultAsync(m => m.IdUser == idUser);

            bool valid = true;

            try
            {
                var bids = _context.Bids.Where(m => m.IdLot == idLot);
                var maxBid = bids.Max(m => m.Price);

                if (amount <= maxBid && amount <= auctionLot.StartPrice) valid = false;
            }
            catch { }


            if (userWallet.Balance >= Convert.ToDouble(amount) && valid)
            {
                try
                {
                    auctionLot.Bids.Add(new Bid()
                    {
                        IdUser = idUser,
                        Price = amount,
                        Time = DateOnly.FromDateTime(DateTime.Now)
                    });

                    await CreateActivity("Ставка", auctionLot.IdLot, idUser); 
                }
                catch { }
            }

            await _context.SaveChangesAsync();

            return Redirect($"GetAuctionItem/{idLot}");
        }

        public async Task<IActionResult> ChangeStatusToReady(int idLot)
        {
            var auctionLot = await _context.Auctionlots
                .FirstOrDefaultAsync(m => m.IdLot == idLot);

            var idUser = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            userSession = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == idUser);

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
            var auctionLot = await _context.Auctionlots
                .FirstOrDefaultAsync(m => m.IdLot == idLot);

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

        [HttpGet]
        public async Task<IActionResult> GetTrandAuctionItems(int page = 0)
        {
            const int PageSize = 3;

            var lots = _context.Auctionlots
                .Where(m => m.Status == "Идёт аукцион").ToList();

            var count = lots.Count;

            var data = lots.Skip(page * PageSize).Take(PageSize).ToList();

            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);

            ViewBag.Page = page;

            return View("TrandAuctionItems", new TrandAuctionItemsModel()
            {
                TrandAuctionLots = lots
            });
        }

        private async Task TransferMoney(decimal amount, string idSender, int idReceiver)
        {
            var walletSender = await _context.Wallets
                .FirstOrDefaultAsync(m => m.IdUser == Convert.ToInt32(idSender));

            var walletReceiver = await _context.Wallets
                .FirstOrDefaultAsync(m => m.IdUser == idReceiver);

            if(amount > 0) { 
                walletSender.Balance -= Convert.ToDouble(amount);
                walletReceiver.Balance += Convert.ToDouble(amount);
            }

        }
    }
}
