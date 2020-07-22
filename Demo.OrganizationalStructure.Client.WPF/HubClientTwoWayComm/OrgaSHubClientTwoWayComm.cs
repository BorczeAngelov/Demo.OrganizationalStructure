using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using Demo.OrganizationalStructure.Common.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Client.WPF.HubClientTwoWayComm
{
    internal class OrgaSHubClientTwoWayComm : IOrgaSHubClientTwoWayComm
    {
        private readonly HubConnection _serverConnection;

        public event Action PongedDemo;
        public event Action<JobRole> JobRoleCreated;
        public event Action<JobRole> JobRoleUpdated;
        public event Action<JobRole> JobRoleDeleted;

        internal OrgaSHubClientTwoWayComm()
        {
            _serverConnection = new HubConnectionBuilder()
                .WithUrl(DemoUrlConstants.LocalHostUrl + DemoUrlConstants.OrgaSHubEndpoint)
                .WithAutomaticReconnect()
                .Build();

            _serverConnection.On(nameof(InvokePongDemo), InvokePongDemo);
            _serverConnection.On<JobRole>(nameof(InvokeCreateJobRole), InvokeCreateJobRole);
            _serverConnection.On<JobRole>(nameof(InvokeUpdateJobRole), InvokeUpdateJobRole);
            _serverConnection.On<JobRole>(nameof(InvokeDeleteJobRole), InvokeDeleteJobRole);

            ServerHubProxy = new ServerHubProxyImp(_serverConnection);
        }

        public IOrgaSHub ServerHubProxy { get; }

        public async Task ConnectWithServerHub()
        {
            await _serverConnection.StartAsync();
        }

        public void InvokePongDemo()
        {
            PongedDemo?.Invoke();
        }

        public void InvokeCreateJobRole(JobRole jobRole)
        {
            JobRoleCreated?.Invoke(jobRole);
        }

        public void InvokeUpdateJobRole(JobRole jobRole)
        {
            JobRoleUpdated?.Invoke(jobRole);
        }

        public void InvokeDeleteJobRole(JobRole jobRole)
        {
            JobRoleDeleted?.Invoke(jobRole);
        }

        #region private class ServerHubProxyImp
        private class ServerHubProxyImp : IOrgaSHub
        {
            private readonly HubConnection _serverConnection;

            internal ServerHubProxyImp(HubConnection serverConnection)
            {
                _serverConnection = serverConnection;
            }

            public Task PingDemo()
            {
                return _serverConnection.InvokeAsync(nameof(PingDemo));
            }

            public Task UpdateJobRole(JobRole jobRole)
            {
                return _serverConnection.InvokeAsync(nameof(UpdateJobRole), jobRole);
            }

            public Task CreateJobRole(JobRole jobRole)
            {
                return _serverConnection.InvokeAsync(nameof(CreateJobRole), jobRole);
            }

            public Task DeleteJobRole(JobRole jobRole)
            {
                return _serverConnection.InvokeAsync(nameof(DeleteJobRole), jobRole);
            }
        }
        #endregion
    }
}
