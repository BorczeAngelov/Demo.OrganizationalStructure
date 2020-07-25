using Demo.OrganizationalStructure.Common.DataModel;
using System;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Common.HubInterfaces
{
    public interface IOrgaSHub
    {
        Task CreateJobRole(JobRole jobRole);
        Task UpdateJobRole(JobRole jobRole);
        Task DeleteJobRole(JobRole jobRole);
        Task CreateEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(Employee employee);
    }

    public interface IOrgaSHubClient
    {
        void InvokeLoadStartingValues(Organisation organisation);

        void InvokeCreateJobRole(JobRole jobRole);
        void InvokeUpdateJobRole(JobRole jobRole);
        void InvokeDeleteJobRole(JobRole jobRole);
        void InvokeCreateEmployee(Employee employee);
        void InvokeUpdateEmployee(Employee employee);
        void InvokeDeleteEmployee(Employee employee);
    }

    public interface IOrgaSHubClientTwoWayComm : IOrgaSHubClient
    {
        event Action<Organisation> LoadStartingValues;

        event Action<JobRole> JobRoleCreated;
        event Action<JobRole> JobRoleUpdated;
        event Action<JobRole> JobRoleDeleted;
        event Action<Employee> EmployeeCreated;
        event Action<Employee> EmployeeUpdated;
        event Action<Employee> EmployeeDeleted;

        IOrgaSHub ServerHubProxy { get; }
        Task ConnectWithServerHub();
    }
}
