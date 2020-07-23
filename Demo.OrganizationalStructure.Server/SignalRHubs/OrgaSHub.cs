using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using Demo.OrganizationalStructure.Server.HubsSharedMemory;
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

        public override Task OnConnectedAsync()
        {
            var jobRoles = OrgaSHubSharedMemorySingleton.GetInstance.GetJobRoles;
            var employees = OrgaSHubSharedMemorySingleton.GetInstance.GetEmployees;
            Clients.Caller.SendAsync(nameof(IOrgaSHubClient.InvokeLoadStartingValues), jobRoles, employees);
            return base.OnConnectedAsync();
        }

        public async Task CreateJobRole(JobRole jobRole)
        {
            OrgaSHubSharedMemorySingleton.GetInstance.AddJobRole(jobRole);
            await Clients.Others.SendAsync(nameof(IOrgaSHubClient.InvokeCreateJobRole), jobRole);
        }

        public async Task UpdateJobRole(JobRole jobRole)
        {
            OrgaSHubSharedMemorySingleton.GetInstance.UpdateJobRole(jobRole);
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokeUpdateJobRole), jobRole);
        }

        public async Task DeleteJobRole(JobRole jobRole)
        {
            OrgaSHubSharedMemorySingleton.GetInstance.RemoveJobRole(jobRole);
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokeDeleteJobRole), jobRole);
        }


        public async Task CreateEmployee(Employee employee)
        {
            OrgaSHubSharedMemorySingleton.GetInstance.AddEmployee(employee);
            await Clients.Others.SendAsync(nameof(IOrgaSHubClient.InvokeCreateEmployee), employee);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            OrgaSHubSharedMemorySingleton.GetInstance.UpdateEmployee(employee);
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokeUpdateEmployee), employee);
        }

        public async Task DeleteEmployee(Employee employee)
        {
            OrgaSHubSharedMemorySingleton.GetInstance.RemoveEmployee(employee);
            await Clients.All.SendAsync(nameof(IOrgaSHubClient.InvokeDeleteEmployee), employee);
        }
    }
}
