using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class Message
{
    public long IdMessage { get; set; }

    public long IdConversation { get; set; }

    public int idSender { get; set; }

    public string Text { get; set; } = null!;

    public virtual Conversation IdConversationNavigation { get; set; } = null!;
}
