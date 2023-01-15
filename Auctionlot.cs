namespace ComplexProject;

public partial class Auctionlot
{
    public long IdLot { get; set; }

    public int IdAuctioneer { get; set; }

    public string Category { get; set; } = null!;

    public string[] AttachmentsLink { get; set; } = null!;

    public decimal StartPrice { get; set; }

    public decimal EndPrice { get; set; }

    public string Type { get; set; } = null!;

    public short ViewerCount { get; set; }

    public string Status { get; set; } = null!;

    public string Winner { get; set; } = null!;

    public short PaybackTime { get; set; }

    public decimal AverageProfit { get; set; }

    public string[] ImageLink { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Description { get; set; }
    public decimal CurrentPrice { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public virtual ICollection<Activity> Activities { get; } = new List<Activity>();

    public virtual ICollection<Auctiontag> Auctiontags { get; } = new List<Auctiontag>();

    public virtual ICollection<Bid> Bids { get; } = new List<Bid>();

    public virtual ICollection<Favouritelot> Favouritelots { get; } = new List<Favouritelot>();

    public virtual User? IdAuctioneerNavigation { get; set; }

}
