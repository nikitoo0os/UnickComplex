using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class ConversationModel : Controller
    {
        public User UserReceiver { get; set; } 
        public User UserSender { get; set; } 
        public List<Message> Messages { get; set; }
        public string Text { get; set; }
        public int idConversation { get; set; }
        public int idSender { get; set; }
        public DateOnly TimeStamp { get; set; }
    }
}
