using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class OrganizationalStructureVM : ObservableBase
    {
        private readonly IOrgaSHubClientTwoWayComm _twoWayComm;
        private EditableItemBaseVM _selectedItem;

        internal OrganizationalStructureVM(IOrgaSHubClientTwoWayComm twoWayComm)
        {
            _twoWayComm = twoWayComm;

            AddJobRoleCommand = new DelegateCommand(CreateNewJobRole);
            AddEmployeeCommand = new DelegateCommand(CreateNewEmployee);

            _twoWayComm.JobRoleCreated += AddNewJobRoleFromServer;
            _twoWayComm.EmployeeCreated += AddNewEmployeeFromServer;

            _twoWayComm.JobRoleDeleted += RemoveLocalJob;
            _twoWayComm.EmployeeDeleted += RemoveLocalEmployee;
        }

        public DelegateCommand AddJobRoleCommand { get; }
        public DelegateCommand AddEmployeeCommand { get; }
        public ObservableCollection<JobRoleVM> JobRoles { get; } = new ObservableCollection<JobRoleVM>();
        public ObservableCollection<EmployeeVM> Employees { get; } = new ObservableCollection<EmployeeVM>();

        public EditableItemBaseVM SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

        private void CreateNewJobRole(object obj)
        {
            var dataModel = new JobRole() { Name = "New job role", EntityKey = Guid.NewGuid() };
            var jobRoleVM = new JobRoleVM(_twoWayComm, dataModel, isNewAndUnsaved: true);
            JobRoles.Add(jobRoleVM);

            SelectedItem = jobRoleVM;
        }

        private void CreateNewEmployee(object obj)
        {
            var dataModel = new Employee() { Name = "New employee", EntityKey = Guid.NewGuid() };
            var employeeVM = new EmployeeVM(_twoWayComm, dataModel, isNewAndUnsaved: true);
            Employees.Add(employeeVM);

            SelectedItem = employeeVM;
        }

        private void AddNewJobRoleFromServer(JobRole jobRole)
        {
            Debug.Assert(!JobRoles.Any(x => x.EntityKey == jobRole.EntityKey));

            var jobRoleVM = new JobRoleVM(_twoWayComm, jobRole);
            JobRoles.Add(jobRoleVM);
        }

        private void AddNewEmployeeFromServer(Employee employee)
        {
            Debug.Assert(!Employees.Any(x => x.EntityKey == employee.EntityKey));

            var employeeVM = new EmployeeVM(_twoWayComm, employee);
            Employees.Add(employeeVM);
        }

        private void RemoveLocalJob(JobRole jobRole)
        {
            var localJobRoleVM = JobRoles.FirstOrDefault(x => x.EntityKey == jobRole.EntityKey);
            if (localJobRoleVM != null)
            {
                JobRoles.Remove(localJobRoleVM);
                if (SelectedItem == localJobRoleVM)
                {
                    SelectedItem = null;
                }
            }
        }

        private void RemoveLocalEmployee(Employee employee)
        {
            var localEmployeeVM = Employees.FirstOrDefault(x => x.EntityKey == employee.EntityKey);
            if (localEmployeeVM != null)
            {
                Employees.Remove(localEmployeeVM);
                if (SelectedItem == localEmployeeVM)
                {
                    SelectedItem = null;
                }
            }
        }
    }
}
