using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Bid
{
    public long IdBid { get; set; }

    public long IdLot { get; set; }

    public int IdUser { get; set; }

    public decimal Price { get; set; }

    public DateOnly Time { get; set; }

    public virtual Auctionlot IdLotNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
