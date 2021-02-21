using Demo.OrganizationalStructure.Common.DataModel;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Client.Web.Services
{
    internal class OrgStructureService
    {
        private readonly HubConnection _serverConnection;

        internal Organisation Organisation { get; private set; }
        internal event Action OrganisationLoaded;

        public OrgStructureService()
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
            Organisation = organisation;
            OrganisationLoaded?.Invoke();
        }

        internal async Task ConnectWithServerAsync()
        {
            await _serverConnection.StartAsync();
        }

        internal async Task ReconnectWithServerAsync()
        {
            await _serverConnection.StopAsync();
            await _serverConnection.StartAsync();
        }
    }
}
