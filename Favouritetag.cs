using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Favouritetag
{
    public long IdFavouriteTag { get; set; }

    public int IdUser { get; set; }

    public short IdTag { get; set; }

    public virtual Tag IdTagNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
