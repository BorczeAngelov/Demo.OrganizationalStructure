using Demo.OrganizationalStructure.Common.DataModel;
using System;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Common.HubInterfaces
{
    public interface IOrgaSHub
    {
        Task PingDemo();

        Task CreateJobRole(JobRole jobRole);
        Task UpdateJobRole(JobRole jobRole);
        Task DeleteJobRole(JobRole jobRole);
    }

    public interface IOrgaSHubClient
    {
        void InvokePongDemo();

        void InvokeCreateJobRole(JobRole jobRole);
        void InvokeUpdateJobRole(JobRole jobRole);
        void InvokeDeleteJobRole(JobRole jobRole);
    }

    public interface IOrgaSHubClientTwoWayComm : IOrgaSHubClient
    {
        event Action PongedDemo;

        event Action<JobRole> JobRoleCreated;
        event Action<JobRole> JobRoleUpdated;
        event Action<JobRole> JobRoleDeleted;

        IOrgaSHub ServerHubProxy { get; }
        Task ConnectWithServerHub();
    }
}
