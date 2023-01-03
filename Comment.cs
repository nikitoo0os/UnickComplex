using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Comment
{
    public long IdComment { get; set; }

    public int IdUser { get; set; }

    public long IdProject { get; set; }

    public string Text { get; set; } = null!;

    public DateOnly Timestamp { get; set; }

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
