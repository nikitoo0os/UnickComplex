using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Auctiontag
{
    public long IdAuctionTag { get; set; }

    public short IdTag { get; set; }

    public long IdLot { get; set; }

    public virtual Auctionlot IdLotNavigation { get; set; } = null!;

    public virtual Tag IdTagNavigation { get; set; } = null!;
}
