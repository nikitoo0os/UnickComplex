using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Tag
{
    public short IdTag { get; set; }

    public string Name { get; set; } = null!;

    public Tag(string name)
    {
        Name = name;
    }

    public virtual ICollection<Auctiontag> Auctiontags { get; } = new List<Auctiontag>();

    public virtual ICollection<Crowdfundingtag> Crowdfundingtags { get; } = new List<Crowdfundingtag>();

    public virtual ICollection<Favouritetag> Favouritetags { get; } = new List<Favouritetag>();
}
