using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class ConversationsModel : Controller
    {
        public Dictionary<int, string> personConversation { get; set; }
        public IOrderedQueryable<Conversation>? Conversations { get; set; }

    }
}
