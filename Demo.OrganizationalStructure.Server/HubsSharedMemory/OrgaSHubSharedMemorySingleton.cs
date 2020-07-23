using Demo.OrganizationalStructure.Common.DataModel;
using System.Collections.Generic;

namespace Demo.OrganizationalStructure.Server.HubsSharedMemory
{
    internal class OrgaSHubSharedMemorySingleton
    {
        private static OrgaSHubSharedMemorySingleton _instance;
        private static readonly object _instanceSyncLock = new object();

        private List<JobRole> _jobRoles;
        private readonly object _jobRolesSyncLock = new object();

        private List<Employee> _employees;
        private readonly object _employeesSyncLock = new object();

        private OrgaSHubSharedMemorySingleton()
        {
            _jobRoles = new List<JobRole>();
            _employees = new List<Employee>();
        }

        internal static OrgaSHubSharedMemorySingleton GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceSyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new OrgaSHubSharedMemorySingleton();
                        }
                    }
                }
                return _instance;
            }
        }

        internal IEnumerable<JobRole> GetJobRoles
        {
            get
            {
                lock (_jobRolesSyncLock)
                {
                    return _jobRoles;
                }
            }
        }

        internal void AddJobRole(JobRole jobRole)
        {
            lock (_jobRolesSyncLock)
            {
                _jobRoles.Add(jobRole);
            }
        }

        internal void UpdateJobRole(JobRole jobRole)
        {
            lock (_jobRolesSyncLock)
            {
                var existingJobRole = _jobRoles.Find(x => x.EntityKey == jobRole.EntityKey);
                if (existingJobRole != null)
                {
                    existingJobRole.Name = jobRole.Name;
                    existingJobRole.Description = jobRole.Description;
                }
            }
        }

        internal void RemoveJobRole(JobRole jobRole)
        {
            lock (_jobRolesSyncLock)
            {
                var existingJobRole = _jobRoles.Find(x => x.EntityKey == jobRole.EntityKey);
                if (existingJobRole != null)
                {
                    _jobRoles.Remove(existingJobRole);
                }
            }
        }


        internal IEnumerable<Employee> GetEmployees
        {
            get
            {
                lock (_employeesSyncLock)
                {
                    return _employees;
                }
            }
        }

        internal void AddEmployee(Employee employee)
        {
            lock (_employeesSyncLock)
            {
                _employees.Add(employee);
            }
        }

        internal void UpdateEmployee(Employee employee)
        {
            lock (_employeesSyncLock)
            {
                var existingEmployee = _employees.Find(x => x.EntityKey == employee.EntityKey);
                if (existingEmployee != null)
                {
                    existingEmployee.Name = employee.Name;
                    existingEmployee.JobRoleEntityKey = employee.JobRoleEntityKey;
                }
            }
        }

        internal void RemoveEmployee(Employee employee)
        {
            lock (_employeesSyncLock)
            {
                var existingEmployee = _employees.Find(x => x.EntityKey == employee.EntityKey);
                if (existingEmployee != null)
                {
                    _employees.Remove(existingEmployee);
                }
            }
        }
    }
}
