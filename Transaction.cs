using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Transaction
{
    public long IdTransaction { get; set; }

    public int IdSender { get; set; }

    public int IdReceiver { get; set; }

    public decimal Quantity { get; set; }

    public virtual Wallet IdSenderNavigation { get; set; } = null!;
}
