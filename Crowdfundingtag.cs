using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Crowdfundingtag
{
    public long IdCrowfundingTag { get; set; }

    public long IdProject { get; set; }

    public short IdTag { get; set; }

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual Tag IdTagNavigation { get; set; } = null!;
}
