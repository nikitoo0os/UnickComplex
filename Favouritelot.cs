using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Favouritelot
{
    public long IdFavouriteLot { get; set; }

    public int IdUser { get; set; }

    public long IdLot { get; set; }

    public virtual Auctionlot IdLotNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
