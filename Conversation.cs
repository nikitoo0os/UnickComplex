using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Conversation
{
    public long IdConversation { get; set; }

    public int IdUser { get; set; }

    public int IdUser1 { get; set; }

    public virtual User IdUser1Navigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; } = new List<Message>();
}
