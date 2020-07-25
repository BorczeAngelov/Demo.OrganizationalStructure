using Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy;
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
        private Organisation _organisationDataModel;

        internal OrganizationalStructureVM(IOrgaSHubClientTwoWayComm twoWayComm)
        {
            SimpleHierarchyVM = new SimpleHierarchyVM(this);

            AddJobRoleCommand = new DelegateCommand(CreateNewJobRole);
            AddEmployeeCommand = new DelegateCommand(CreateNewEmployee);

            _twoWayComm = twoWayComm;
            _twoWayComm.JobRoleCreated += AddNewJobRoleFromServer;
            _twoWayComm.EmployeeCreated += AddNewEmployeeFromServer;

            _twoWayComm.JobRoleDeleted += RemoveLocalJob;
            _twoWayComm.EmployeeDeleted += RemoveLocalEmployee;

            _twoWayComm.LoadStartingValues += OnLoadStartingValues;
        }

        public DelegateCommand AddJobRoleCommand { get; }
        public DelegateCommand AddEmployeeCommand { get; }
        public SimpleHierarchyVM SimpleHierarchyVM { get; }
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
                    OnPropertyChanged(nameof(SelectedJobRole));
                    OnPropertyChanged(nameof(SelectedEmployee));
                }
            }
        }

        public JobRoleVM SelectedJobRole { get => SelectedItem as JobRoleVM; set => SelectedItem = value; }
        public EmployeeVM SelectedEmployee { get => SelectedItem as EmployeeVM; set => SelectedItem = value; }

        private void CreateNewJobRole(object obj)
        {
            var startingName = "Job role " + (JobRoles.Count + 1);
            var dataModel = new JobRole() { Name = startingName, EntityKey = Guid.NewGuid() };
            var jobRoleVM = new JobRoleVM(_twoWayComm, dataModel, JobRoles, isNewAndUnsaved: true);
            JobRoles.Add(jobRoleVM);

            SelectedItem = jobRoleVM;
        }

        private void CreateNewEmployee(object obj)
        {
            var startingName = "Employee" + (Employees.Count + 1);
            var dataModel = new Employee() { Name = startingName, EntityKey = Guid.NewGuid() };
            var employeeVM = new EmployeeVM(_twoWayComm, dataModel, JobRoles, isNewAndUnsaved: true);
            Employees.Add(employeeVM);

            SelectedItem = employeeVM;
        }

        private void AddNewJobRoleFromServer(JobRole jobRole)
        {
            Debug.Assert(!JobRoles.Any(x => x.EntityKey == jobRole.EntityKey));

            var jobRoleVM = new JobRoleVM(_twoWayComm, jobRole, JobRoles);
            JobRoles.Add(jobRoleVM);
        }

        private void AddNewEmployeeFromServer(Employee employee)
        {
            Debug.Assert(!Employees.Any(x => x.EntityKey == employee.EntityKey));

            var employeeVM = new EmployeeVM(_twoWayComm, employee, JobRoles);
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

        private void OnLoadStartingValues(Organisation organisation)
        {
            _organisationDataModel = organisation;

            foreach (var jobRole in organisation.JobRoles)
            {
                AddNewJobRoleFromServer(jobRole);
            }

            foreach (var employee in organisation.Employees)
            {
                AddNewEmployeeFromServer(employee);
            }
        }
    }
}
