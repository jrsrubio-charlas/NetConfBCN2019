using System.Threading.Tasks;

namespace NetConBCN2019Demo2.API.Services
{
    public interface ISendToEventHubService
    {
        Task SendMessageToEventHub<T>(string ehEntityPath, T model);
    }
}
