using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Donation
{
    public long IdDonate { get; set; }

    public long IdProject { get; set; }

    public int IdUser { get; set; }

    public decimal Quantity { get; set; }

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
