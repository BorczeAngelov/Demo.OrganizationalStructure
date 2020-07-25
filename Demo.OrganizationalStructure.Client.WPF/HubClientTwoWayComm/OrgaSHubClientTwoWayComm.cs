using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using Demo.OrganizationalStructure.Common.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public event Action<IEnumerable<JobRole>, IEnumerable<Employee>> LoadStartingValues;

        internal OrgaSHubClientTwoWayComm()
        {
            _serverConnection = new HubConnectionBuilder()
                .WithUrl(DemoUrlConstants.LocalHostUrl + DemoUrlConstants.OrgaSHubEndpoint)
                .WithAutomaticReconnect()
                .Build();

            _serverConnection.On<JobRole>(nameof(InvokeCreateJobRole), InvokeCreateJobRole);
            _serverConnection.On<JobRole>(nameof(InvokeUpdateJobRole), InvokeUpdateJobRole);
            _serverConnection.On<JobRole>(nameof(InvokeDeleteJobRole), InvokeDeleteJobRole);
            _serverConnection.On<Employee>(nameof(InvokeCreateEmployee), InvokeCreateEmployee);
            _serverConnection.On<Employee>(nameof(InvokeUpdateEmployee), InvokeUpdateEmployee);
            _serverConnection.On<Employee>(nameof(InvokeDeleteEmployee), InvokeDeleteEmployee);
            _serverConnection.On<IEnumerable<JobRole>, IEnumerable<Employee>>(nameof(InvokeLoadStartingValues), InvokeLoadStartingValues);

            ServerHubProxy = new ServerHubProxyImp(_serverConnection);
        }

        public IOrgaSHub ServerHubProxy { get; }

        public async Task ConnectWithServerHub()
        {
            await _serverConnection.StartAsync();
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


        public void InvokeCreateEmployee(Employee employee)
        {
            EmployeeCreated?.Invoke(employee);
        }

        public void InvokeUpdateEmployee(Employee employee)
        {
            EmployeeUpdated?.Invoke(employee);
        }

        public void InvokeDeleteEmployee(Employee employee)
        {
            EmployeeDeleted?.Invoke(employee);
        }

        public void InvokeLoadStartingValues(IEnumerable<JobRole> jobRoles, IEnumerable<Employee> employees)
        {
            LoadStartingValues?.Invoke(jobRoles, employees);
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
