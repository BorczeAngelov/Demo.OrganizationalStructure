using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Server.StartingData;
using System.Collections.Generic;

namespace Demo.OrganizationalStructure.Server.HubsSharedMemory
{
    internal class OrgaSHubSharedMemorySingleton
    {
        private static OrgaSHubSharedMemorySingleton _instance;
        private static readonly object _instanceSyncLock = new object();

        private Organisation _organisation;
        private readonly object _organisationSyncLock = new object();
        private readonly object _jobRolesSyncLock = new object();
        private readonly object _employeesSyncLock = new object();

        private OrgaSHubSharedMemorySingleton()
        {
            _organisation = StartingDataProvider.GetOrganisation();
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

        internal Organisation Organisation
        {
            get
            {
                lock (_organisationSyncLock)
                {
                    return _organisation;
                }
            }
            set
            {
                lock (_organisationSyncLock)
                {                    
                    _organisation = value;
                }
            }
        }

        internal void AddJobRole(JobRole jobRole)
        {
            lock (_jobRolesSyncLock)
            {
                _organisation.JobRoles.Add(jobRole);
            }
        }

        internal void UpdateJobRole(JobRole jobRole)
        {
            lock (_jobRolesSyncLock)
            {
                var existingJobRole = _organisation.JobRoles.Find(x => x.EntityKey == jobRole.EntityKey);
                if (existingJobRole != null)
                {
                    existingJobRole.Name = jobRole.Name;
                    existingJobRole.Description = jobRole.Description;
                    existingJobRole.UpperHierarchyJobRoleKey = jobRole.UpperHierarchyJobRoleKey;
                }
            }
        }

        internal void RemoveJobRole(JobRole jobRole)
        {
            lock (_jobRolesSyncLock)
            {
                var existingJobRole = _organisation.JobRoles.Find(x => x.EntityKey == jobRole.EntityKey);
                if (existingJobRole != null)
                {
                    _organisation.JobRoles.Remove(existingJobRole);
                }
            }
        }

        internal void AddEmployee(Employee employee)
        {
            lock (_employeesSyncLock)
            {
                _organisation.Employees.Add(employee);
            }
        }

        internal void UpdateEmployee(Employee employee)
        {
            lock (_employeesSyncLock)
            {
                var existingEmployee = _organisation.Employees.Find(x => x.EntityKey == employee.EntityKey);
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
                var existingEmployee = _organisation.Employees.Find(x => x.EntityKey == employee.EntityKey);
                if (existingEmployee != null)
                {
                    _organisation.Employees.Remove(existingEmployee);
                }
            }
        }
    }
}
