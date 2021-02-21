using Demo.OrganizationalStructure.Common.DataModel;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Client.Web.Data
{
    internal class OrganizationalStructureSignalRHub
    {
        private readonly HubConnection _serverConnection;

        internal event Action<Organisation> LoadOrganisation;

        public OrganizationalStructureSignalRHub()
        {
            var serverHubUrl = "http://localhost:5000/OrgaSHub";

            _serverConnection = new HubConnectionBuilder()
                .WithUrl(serverHubUrl)
                .WithAutomaticReconnect()
                .Build();

            _serverConnection.On<Organisation>(nameof(InvokeLoadOrganisation), InvokeLoadOrganisation);
        }

        private void InvokeLoadOrganisation(Organisation organisation)
        {
            LoadOrganisation?.Invoke(organisation);
        }

        internal async Task ConnectWithServerAsync()
        {
            await _serverConnection.StartAsync();
        }
    }
}
