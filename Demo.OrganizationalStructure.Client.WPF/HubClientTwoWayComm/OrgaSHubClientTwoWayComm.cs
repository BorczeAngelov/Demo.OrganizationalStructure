using Demo.OrganizationalStructure.Common.HubInterfaces;
using Demo.OrganizationalStructure.Common.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Client.WPF.HubClientTwoWayComm
{
    internal class OrgaSHubClientTwoWayComm : IOrgaSHubClientTwoWayComm
    {
        private readonly HubConnection _serverConnection;

        internal OrgaSHubClientTwoWayComm()
        {
            _serverConnection = new HubConnectionBuilder()
                .WithUrl(DemoUrlConstants.LocalHostUrl + DemoUrlConstants.OrgaSHubEndpoint)
                .WithAutomaticReconnect()
                .Build();

            ServerHubProxy = new ServerHubProxyImp(_serverConnection);
        }

        public IOrgaSHub ServerHubProxy { get; }

        public async Task ConnectWithServerHub()
        {
            await _serverConnection.StartAsync();
        }

        #region private class ServerHubProxyImp
        private class ServerHubProxyImp : IOrgaSHub
        {
            private readonly HubConnection _serverConnection;

            internal ServerHubProxyImp(HubConnection serverConnection)
            {
                _serverConnection = serverConnection;
            }
        }
        #endregion
    }
}
