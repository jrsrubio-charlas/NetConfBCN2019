using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NetConfBCN2019Demo1.Hubs
{
    public class MessageHub : Hub
    {
        //Aquí se implentarían los métodos para ser llamados desde el cliente, por ejemplo en un chat ;-)
        public Task BroadcastMessage(string message)
        {
            return Clients.All.SendAsync("new-message", message);
        }
    }
}
