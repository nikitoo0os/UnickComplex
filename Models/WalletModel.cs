using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class WalletModel : Controller
    {
        public List<Wallet>? Wallet { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
