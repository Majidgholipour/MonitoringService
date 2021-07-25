using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MonitoringService.Hubs
{
    [HubName("myHub")]
    public class MyHub : Hub
    {
        [HubMethodName("sendMessages")]
        public static void SendMessages()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.updateMessages();
        }
    }
}