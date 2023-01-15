using ComplexProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ComplexProject.Controllers
{
    public class AuctionController : Controller
    {
        private readonly UnickDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuctionController(UnickDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult CreateAuctionItem()
        {
            return View("CreateAuctionItem");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuctionLot([Bind("Title,Description,AverageProfit,Category,StartPrice,EndPrice,Type,Location,FileModel")] CreateAuctionlotModel createModel)
        {
            if (!ModelState.IsValid)
            {
                Auctionlot model = new Auctionlot();
                model.Title = createModel.Title;
                model.Description = createModel.Description;
                model.AverageProfit = createModel.AverageProfit;
                model.Category = createModel.Category;
                model.StartPrice = createModel.StartPrice;
                model.EndPrice = createModel.EndPrice;
                model.Type = createModel.Type;
                model.Location = createModel.Location;
                model.IdAuctioneer = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);
                model.Status = "На проверке";
                model.Winner = "";
                model.AttachmentsLink = new string[] { " " };
                model.ImageLink = new string[] { " " };


                if (createModel.FileModel == null)
                {
                    var EntityModel = _context.Add(model);
                    await _context.SaveChangesAsync();

                    long id = EntityModel.Entity.IdLot;

                    string wwwrootpath = _webHostEnvironment.WebRootPath;

                    string SubDirPath = $"AuctionLot{id}";

                    DirectoryInfo directoryInfo = new DirectoryInfo(wwwrootpath + "/AuctionLots");
                    if (directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }

                    directoryInfo.CreateSubdirectory(SubDirPath);

                    string FileName = Path.GetFileNameWithoutExtension(createModel.FileModel.FileTitle);
                    string extension = Path.GetExtension(createModel.FileModel.FileTitle);

                    createModel.FileModel.FileTitle = FileName + id + extension;

                    var paths = _context.Auctionlots
                        .FirstOrDefault(m => m.IdLot == id).AttachmentsLink
                        .ToList();

                    paths.Add(createModel.FileModel.FileTitle);

                    model.AttachmentsLink = paths.ToArray();


                    string path = Path.Combine(wwwrootpath + "/AuctionLots/" + SubDirPath + "/" + createModel.FileModel.FileTitle);

                    using (var FileStream = new FileStream(path, FileMode.Create))
                    {
                        await createModel.FileModel.File.CopyToAsync(FileStream);
                    }
                    await CreateActivity("Создание лота", id, model.IdAuctioneer);
                    _context.Auctionlots.Update(model);

                    var user = await _context.Users
                        .FirstOrDefaultAsync(m => m.IdUser == Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]));
                    
                    user.Auctionlots.Add(model);
                    await CreateTag(model.Category);

                    await _context.SaveChangesAsync();

                    return View("HomePage");
                }

               
                //var wallet = await _context.Wallets
                //    .FirstOrDefaultAsync(m => m.IdUser == user.IdUser);
                //var auctionLots = _context.Auctionlots
                //    .Where(m => m.IdAuctioneer == user.IdUser);   
            }

            return Redirect("/");
        }

        public async Task<IActionResult> GetAuctionItem(long id)
        {

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

                    await TransferMoney(auctionLot.EndPrice, Convert.ToInt32(auctionLot.Winner), auctionLot.IdAuctioneer);

                    await _context.SaveChangesAsync();
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
                var bids = _context.Bids
                    .Where(m => m.IdLot == idLot);

                var maxBid = bids
                    .Max(m => m.Price);

                if (amount <= maxBid || amount <= auctionLot.StartPrice) 
                    valid = false;
            }
            catch { }


            if (userWallet.Balance >= Convert.ToDouble(amount) && valid)
            {

                if (amount >= auctionLot.EndPrice)
                {
                    await SetWinner(auctionLot.IdLot, idUser);
                    await TransferMoney(amount, idUser, auctionLot.IdAuctioneer);
                    await CreateActivity("Завершение аукциона", auctionLot.IdLot, auctionLot.IdAuctioneer);

                    return Redirect($"GetAuctionItem/{idLot}");
                }
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

        private async Task TransferMoney(decimal amount, int idSender, int idReceiver)
        {
            var walletSender = await _context.Wallets
                .FirstOrDefaultAsync(m => m.IdUser == Convert.ToInt32(idSender));

            var walletReceiver = await _context.Wallets
                .FirstOrDefaultAsync(m => m.IdUser == idReceiver);

            if (amount > 0)
            {
                walletSender.Balance -= Convert.ToDouble(amount);
                walletReceiver.Balance += Convert.ToDouble(amount);
            }

        }
        private async Task SetWinner(long idLot, int idWinner)
        {
            var auctionLot = await _context.Auctionlots
                .FirstOrDefaultAsync(m => m.IdLot== idLot);
            auctionLot.Winner = idWinner.ToString();
            auctionLot.Status = "Завершено";

            _context.Update(auctionLot);
            await _context.SaveChangesAsync();

            await CreateActivity("Назначение победителя", idLot, idWinner);
        }

        private async Task CreateTag(string name)
        {
            var currTag = await _context.Tags
                .FirstOrDefaultAsync(m => m.Name == name);

            if (currTag == null)
            {
                _context.Tags.Add(new Tag(name));
            }

        }
        private async Task CreateActivity(string type, long idLot, int idUser)
        {
            await _context.Activities.AddAsync(new Activity()
            {
                IdLot = idLot,
                IdUser = idUser,
                Type = type,
                Time = DateTime.Now.ToUniversalTime()
        });
        }
    }
}
