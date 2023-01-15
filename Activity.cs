using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Activity
{
    public long IdActivity { get; set; }

    public long IdLot { get; set; }

    public int IdUser { get; set; }

    public string Type { get; set; } = null!;

    public DateTime Time { get; set; }

    public virtual Auctionlot IdLotNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
