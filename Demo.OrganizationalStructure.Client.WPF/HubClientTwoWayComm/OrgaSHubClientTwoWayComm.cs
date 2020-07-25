using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Demo.OrganizationalStructure.Client.WPF.HubClientTwoWayComm
{
    internal class OrgaSHubClientTwoWayComm : IOrgaSHubClientTwoWayComm
    {
        private readonly HubConnection _serverConnection;

        public event Action<JobRole> JobRoleCreated;
        public event Action<JobRole> JobRoleUpdated;
        public event Action<JobRole> JobRoleDeleted;
        public event Action<Employee> EmployeeCreated;
        public event Action<Employee> EmployeeUpdated;
        public event Action<Employee> EmployeeDeleted;
        public event Action<Organisation> LoadStartingValues;

        internal OrgaSHubClientTwoWayComm(string serverHubUrl)
        {
            _serverConnection = new HubConnectionBuilder()
                .WithUrl(serverHubUrl)
                .WithAutomaticReconnect()
                .Build();

            _serverConnection.On<JobRole>(nameof(InvokeCreateJobRole), InvokeCreateJobRole);
            _serverConnection.On<JobRole>(nameof(InvokeUpdateJobRole), InvokeUpdateJobRole);
            _serverConnection.On<JobRole>(nameof(InvokeDeleteJobRole), InvokeDeleteJobRole);
            _serverConnection.On<Employee>(nameof(InvokeCreateEmployee), InvokeCreateEmployee);
            _serverConnection.On<Employee>(nameof(InvokeUpdateEmployee), InvokeUpdateEmployee);
            _serverConnection.On<Employee>(nameof(InvokeDeleteEmployee), InvokeDeleteEmployee);
            _serverConnection.On<Organisation>(nameof(InvokeLoadStartingValues), InvokeLoadStartingValues);

            ServerHubProxy = new ServerHubProxyImp(_serverConnection);
        }

        public IOrgaSHub ServerHubProxy { get; }

        public async Task ConnectWithServerHub()
        {
            await _serverConnection.StartAsync();
        }

        public void InvokeCreateJobRole(JobRole jobRole)
        {
            Dispatcher.CurrentDispatcher.Invoke(() => JobRoleCreated?.Invoke(jobRole));
        }

        public void InvokeUpdateJobRole(JobRole jobRole)
        {
            Dispatcher.CurrentDispatcher.Invoke(() => JobRoleUpdated?.Invoke(jobRole));
        }

        public void InvokeDeleteJobRole(JobRole jobRole)
        {
            Dispatcher.CurrentDispatcher.Invoke(() => JobRoleDeleted?.Invoke(jobRole));
        }


        public void InvokeCreateEmployee(Employee employee)
        {
            Dispatcher.CurrentDispatcher.Invoke(() => EmployeeCreated?.Invoke(employee));
        }

        public void InvokeUpdateEmployee(Employee employee)
        {
            Dispatcher.CurrentDispatcher.Invoke(() => EmployeeUpdated?.Invoke(employee));
        }

        public void InvokeDeleteEmployee(Employee employee)
        {
            Dispatcher.CurrentDispatcher.Invoke(() => EmployeeDeleted?.Invoke(employee));
        }

        public void InvokeLoadStartingValues(Organisation organisation)
        {
            Dispatcher.CurrentDispatcher.Invoke(() => LoadStartingValues?.Invoke(organisation));
        }

        #region private class ServerHubProxyImp
        private class ServerHubProxyImp : IOrgaSHub
        {
            private readonly HubConnection _serverConnection;

            internal ServerHubProxyImp(HubConnection serverConnection)
            {
                _serverConnection = serverConnection;
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

            public Task UpdateEmployee(Employee employee)
            {
                return _serverConnection.InvokeAsync(nameof(UpdateEmployee), employee);
            }

            public Task CreateEmployee(Employee employee)
            {
                return _serverConnection.InvokeAsync(nameof(CreateEmployee), employee);
            }

            public Task DeleteEmployee(Employee employee)
            {
                return _serverConnection.InvokeAsync(nameof(DeleteEmployee), employee);
            }
        }
        #endregion
    }
}
