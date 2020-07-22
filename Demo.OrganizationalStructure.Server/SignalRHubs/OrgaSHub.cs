﻿using Demo.OrganizationalStructure.Common.DataModel;
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


        public async Task CreateJobRole(JobRole jobRole)
        {
            await Clients.Others.SendAsync(nameof(IOrgaSHubClient.InvokeCreateJobRole), jobRole);
        }

        public async Task UpdateJobRole(JobRole jobRole)
        {
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokeUpdateJobRole), jobRole);
        }

        public async Task DeleteJobRole(JobRole jobRole)
        {
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokeDeleteJobRole), jobRole);
        }


        public async Task CreateEmployee(Employee employee)
        {
            await Clients.Others.SendAsync(nameof(IOrgaSHubClient.InvokeCreateEmployee), employee);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokeUpdateEmployee), employee);
        }

        public async Task DeleteEmployee(Employee employee)
        {
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokeDeleteEmployee), employee);
        }
    }
}
