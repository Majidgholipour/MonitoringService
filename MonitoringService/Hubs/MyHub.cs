using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MonitoringService.Hubs
{
    [HubName("MyHub")]
    public class MyHub : Hub
    {
        [HubMethodName("sendUptodateInformation")]
        public static void SendUptodateInformation(string action)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

            // the updateStudentInformation method will update the connected client about any recent changes in the server data
            context.Clients.All.updateInformation(action);
        }
    }
}