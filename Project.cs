using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Project
{
    public long IdProject { get; set; }

    public string Title { get; set; } = null!;

    public int IdUser { get; set; }

    public string Description { get; set; } = null!;

    public string Category { get; set; } = null!;

    public DateOnly StartDateDonate { get; set; }

    public DateOnly EndDateDonate { get; set; }

    public decimal DonateAmount { get; set; }

    public string CoverLink { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Crowdfundingtag> Crowdfundingtags { get; } = new List<Crowdfundingtag>();

    public virtual ICollection<Donation> Donations { get; } = new List<Donation>();

    public virtual User IdUserNavigation { get; set; } = null!;
}
