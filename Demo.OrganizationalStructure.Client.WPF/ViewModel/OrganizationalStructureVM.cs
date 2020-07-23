using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;
using System.Collections.ObjectModel;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class OrganizationalStructureVM
    {
        private readonly IOrgaSHubClientTwoWayComm _twoWayComm;

        internal OrganizationalStructureVM(IOrgaSHubClientTwoWayComm twoWayComm)
        {
            _twoWayComm = twoWayComm;

            AddJobRoleCommand = new DelegateCommand(CreateNewJobRole);
            AddEmployeeCommand = new DelegateCommand(CreateNewEmployee);

            _twoWayComm.JobRoleCreated += AddNewJobRoleFromServer;
            _twoWayComm.EmployeeCreated += AddNewEmployeeFromServer;
        }

        public DelegateCommand AddJobRoleCommand { get; }
        public DelegateCommand AddEmployeeCommand { get; }
        public ObservableCollection<JobRoleVM> JobRoles { get; } = new ObservableCollection<JobRoleVM>();
        public ObservableCollection<EmployeeVM> Employees { get; } = new ObservableCollection<EmployeeVM>();

        private void CreateNewJobRole(object obj)
        {
            var dataModel = new JobRole() { Name = "New job role", EntityKey = Guid.NewGuid() };
            var jobRoleVM = new JobRoleVM(_twoWayComm, dataModel, isNewAndUnsaved: true);
            JobRoles.Add(jobRoleVM);
        }

        private void CreateNewEmployee(object obj)
        {
            var dataModel = new Employee() { Name = "New employee", EntityKey = Guid.NewGuid() };
            var employeeVM = new EmployeeVM(_twoWayComm, dataModel, isNewAndUnsaved: true);
            Employees.Add(employeeVM);
        }

        private void AddNewJobRoleFromServer(JobRole jobRole)
        {
            var jobRoleVM = new JobRoleVM(_twoWayComm, jobRole);
            JobRoles.Add(jobRoleVM);
        }

        private void AddNewEmployeeFromServer(Employee employee)
        {
            var employeeVM = new EmployeeVM(_twoWayComm, employee);
            Employees.Add(employeeVM);
        }
    }
}
