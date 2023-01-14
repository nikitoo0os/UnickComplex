using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ComplexProject.Controllers
{
    public class ChatHub : Hub
    {
        private UnickDbContext _context;
        public ChatHub(UnickDbContext context) { 
            _context = context;
        }

        public async Task SendMessage(string idSender, string message)
        {
            Message msg = new Message();
            msg.idSender = Convert.ToInt32(idSender);
            msg.Text = message;

            await _context.AddAsync(msg);
            await _context.AddRangeAsync(message);

            await Clients.All.SendAsync("ReceiveMessage", idSender, message);
        }
    }
}
