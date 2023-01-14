using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class ConversationsModel : Controller
    {
        public Dictionary<int, string> personConversation { get; set; } = new Dictionary<int, string>();
        public List<Conversation>? Conversations { get; set; }
        public string lastMsg { get; set; }

        public int IdSender { get; set; }
        public int IdReceiver { get; set; }
        public string Message { get; set; }

    }
}
