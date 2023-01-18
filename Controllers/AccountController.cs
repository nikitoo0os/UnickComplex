using ComplexProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace ComplexProject.Controllers
{
    public class AccountController : Controller, IController
    {
        private readonly UnickDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(UnickDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Registration()
        {
            if(HttpContext.Request.Cookies["Unick_User_Id"] != null)
            {
                return LoginWithCookies();
            }
            else
            {
                return GetAuthPage();
            }
            
        }

        public async Task<IActionResult> GetAuthPage()
        {
            return View("AuthPage");
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("User,ImageModel")] ProfileViewModel profileModel)
        {
            var idUser = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_Id"]);

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == idUser);

            user.FirstName = profileModel.User.FirstName;
            user.SecondName = profileModel.User.SecondName;
            user.DateOfBirth = profileModel.User.DateOfBirth;

            if (profileModel.ImageModel != null)
            {
                long id = idUser;

                string wwwrootpath = _webHostEnvironment.WebRootPath;

                string SubDirPath = $"User{id}";

                DirectoryInfo directoryInfo = new DirectoryInfo(wwwrootpath + "/User");
                if (directoryInfo.Exists) directoryInfo.Create();

                directoryInfo.CreateSubdirectory(SubDirPath);

                string FileName = profileModel.ImageModel.File.FileName;
                int length = FileName.Length - 4;
                string extension = FileName.Substring(length, 4);
                string fileNameWithoutExtension = FileName.Substring(0, length);

                profileModel.ImageModel.Title = fileNameWithoutExtension + id + extension;

                string newPath = "/User/" + SubDirPath + "/" + profileModel.ImageModel.Title;

                string path = Path
                    .Combine(wwwrootpath + newPath);

                string sourceFile = "createModel.ImageModel.Title";

                using (var FileStream = new FileStream(path, FileMode.Create))
                {
                    await profileModel.ImageModel.File.CopyToAsync(FileStream);
                }

                user.PathImage = newPath;
                
            }

            _context.Update(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch { }

            ProfileViewModel profile = new ProfileViewModel();
            profile.User = user;

            return Redirect("/Account/LoginWithCookies");
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
            }
            return View("HomePage");
        }
        private User userSession;
        public async Task<IActionResult> LoginWithCookies()
        {
            if (HttpContext.Request.Cookies["Unick_User_Id"] == null)
            {
                return View("AuthPage");
            }

            int id = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);
            HttpContext.Session.SetInt32("idUser", id);
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

                try
                {
                    if (userSession.Role == "admin")
                    {
                        HttpContext.Session.SetInt32("idUser", userSession.IdUser);
                        return View("AdminPanel", new AdminPanelModel()
                        {
                            User = userSession,
                            AuctionLots = _context.Auctionlots.Where(m => m.Status == "На проверке")
                        });
                    }
                }
                catch
                {
                    return View("AuthPageErr");
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

                    HttpContext.Session.SetInt32("idUser", userSession.IdUser);

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
            HttpContext.Session.Clear();

            return Redirect("/");
        }

        public IActionResult EditAccountProfile()
        {
            var user = _context.Users
               .FirstOrDefault(m => m.IdUser == Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]));

            return View("EditProfile", new ProfileViewModel()
            {
                User = user
            });
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
            cookieOptions.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Append(key, value, cookieOptions);

            return View("HomePage");
        }
        public void DeleteCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
        public async Task<IActionResult> get(int? id)
        { 
            int? temp = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            if (temp != null && temp != 0)
            {
                id = temp;
            }
            else
            {
                return View("AuthPage");
            }
            
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == id);

            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(m => m.IdUser == id);

            var auctionLots = _context.Auctionlots
                .Where(m => m.IdAuctioneer == id);

            var idProfile = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            return View("Profile", new ProfileViewModel()
            {
                User = user,
                Wallet = wallet,
                AuctionLot = auctionLots,
                IdProfile = idProfile
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

        public async Task<IActionResult> MyMessages()
        {
            //int idUser = HttpContext.Session.GetInt32("idUser") ?? 0;
            int idUser = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_Id"]);

            var userConversations = _context.Conversations
                .Where(m => m.IdUser == idUser || m.IdUser1 == idUser)
                .ToList();

            ConversationsModel convModel = new ConversationsModel();
            convModel.Conversations = userConversations;

            foreach (var conv in userConversations)
            {
                var user = _context.Users
                    .FirstOrDefault(m => m.IdUser == conv.IdUser);

                string fullName = user.FirstName + " " + user.SecondName;

                if (!convModel.personConversation.ContainsKey(idUser))
                    convModel.personConversation.Add(idUser, fullName);
                
                var messages = _context.Messages.Where(m => m.IdConversation == conv.IdConversation);

                if(messages.Any())
                {
                    convModel.lastMsg = messages.OrderBy(m => m.IdMessage).ToList().Last().Text;
                }  
            }
            return View("ConversationList", convModel);
        }

        [HttpGet]
        public async Task<IActionResult> NewMessage(int idUser)
        {
            var idSender = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            Conversation conversation = new Conversation();
            var curCov = await _context.Conversations
                .FirstOrDefaultAsync(m => m.IdUser == idUser & m.IdUser1 == idSender || m.IdUser == idSender & m.IdUser1 == idUser);
           
            if(curCov != null)
                conversation = curCov;
            
            else
            {
                conversation.IdUser = idSender;
                conversation.IdUser1 = idUser;

                await _context.Conversations.AddAsync(conversation);
                await _context.SaveChangesAsync();
            }

            ConversationModel convModel = new ConversationModel();

            convModel.UserSender = await _context.Users.FirstOrDefaultAsync(m => m.IdUser == idSender);

            convModel.UserReceiver = await _context.Users.FirstOrDefaultAsync(m => m.IdUser == idUser);

            convModel.Messages = _context.Messages
                .Where(m => m.IdConversation == curCov.IdConversation).ToList();            
           
            return View("Conversation", convModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetConversation(int idConv)
        {
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(m => m.IdConversation == idConv);

            var messages = _context.Messages
                .Where(m => m.IdConversation == idConv)
                .OrderBy(m => m.IdMessage)
                .ToList();

            var user1 = await _context.Users.FirstOrDefaultAsync(m => m.IdUser == conversation.IdUser);
            var user2 = await _context.Users.FirstOrDefaultAsync(m => m.IdUser == conversation.IdUser1);

            ConversationModel convModel = new ConversationModel();
            convModel.Messages = messages;
            convModel.UserSender = user1;
            convModel.UserReceiver = user2;
            convModel.idConversation = Convert.ToInt32(conversation.IdConversation);

            return View("Conversation", convModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage([Bind("idSender,idReceiver,Text,idConversation")]ConversationModel model)
        {
            var idSender = Convert.ToInt32(HttpContext.Request.Cookies["Unick_User_ID"]);

            Message message = new Message();
            message.Text = model.Text;
            message.idSender = idSender;
            message.IdConversation = model.idConversation;

            await _context.Messages.AddAsync(message);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch{ }

            return Redirect($"GetConversation?idConv={model.idConversation}");
        }

        public async Task<IActionResult> GetCategories()
        {
            CategoryModel categoryModel = new CategoryModel();

            var popularTags = (from at in _context.Auctiontags
                               join t in _context.Tags on at.IdTag equals t.IdTag
                               group t by t.Name into g
                               orderby g.Count() descending
                               select g.Key).ToList();

            categoryModel.Categories = popularTags;

            return View("Category", categoryModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(string category)
        {
            var lots = _context.Auctionlots
                .Where(m => m.Category == category)
                .ToList();

            TrandAuctionItemsModel model = new TrandAuctionItemsModel();
            model.TrandAuctionLots = lots;
            model.curTag.Add(category);

            return View("TrandAuctionItems", model);
        }

        
    }
}
