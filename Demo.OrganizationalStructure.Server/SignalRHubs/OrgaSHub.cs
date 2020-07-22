using Demo.OrganizationalStructure.Common.HubInterfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Server.SignalRHubs
{
    internal class OrgaSHub : Hub, IOrgaSHub
    {
        public async Task PingDemo()
        {
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokePongDemo));
        }
    }
}
