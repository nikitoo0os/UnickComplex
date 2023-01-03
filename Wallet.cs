using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Wallet
{
    public int IdWallet { get; set; }

    public int IdUser { get; set; }

    public double Balance { get; set; } = 0;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}
