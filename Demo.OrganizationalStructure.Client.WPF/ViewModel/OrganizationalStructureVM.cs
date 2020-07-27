using Demo.OrganizationalStructure.Client.WPF.AddonFeatures.ImportExport;
using Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy;
using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class OrganizationalStructureVM : ObservableBase
    {
        private readonly IOrgaSHubClientTwoWayComm _twoWayComm;
        private readonly Organisation _organisationDataModel;

        private EditableItemBaseVM _selectedItem;
        private ImportExportImp _importExportImp;

        internal OrganizationalStructureVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
             ImportExportImp importExportImp)
        {
            _importExportImp = importExportImp;
            _organisationDataModel = new Organisation() { JobRoles = new List<JobRole>(), Employees = new List<Employee>() };

            SimpleHierarchyVM = new SimpleHierarchyVM(this);

            ExportCommand = new DelegateCommand(
                arg =>
                {
                    foreach (var item in JobRoles) { item.SaveCommand.Execute(null); }
                    foreach (var item in Employees) { item.SaveCommand.Execute(null); }

                    _importExportImp.Export(_organisationDataModel);
                });

            ImportCommand = new DelegateCommand(
                arg =>
                    {
                        var organisation = _importExportImp.Import();
                        if (organisation != null)
                        {
                            ImportNewOrganisation(organisation);
                        }
                    });

            AddJobRoleCommand = new DelegateCommand(CreateNewJobRole);
            AddEmployeeCommand = new DelegateCommand(CreateNewEmployee);

            _twoWayComm = twoWayComm;
            _twoWayComm.JobRoleCreated += AddNewJobRoleFromServer;
            _twoWayComm.EmployeeCreated += AddNewEmployeeFromServer;

            _twoWayComm.JobRoleDeleted += RemoveLocalJob;
            _twoWayComm.EmployeeDeleted += RemoveLocalEmployee;

            _twoWayComm.LoadOrganisation += OnLoadStartingValues;
        }

        public SimpleHierarchyVM SimpleHierarchyVM { get; }
        public DelegateCommand ImportCommand { get; }
        public DelegateCommand ExportCommand { get; }


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
            var jobRoleVM = new JobRoleVM(_twoWayComm, dataModel, JobRoles, isNew: true);

            var startingUpperHierarchyJobRole = SelectedJobRole;
            jobRoleVM.UpperHierarchyJobRole = startingUpperHierarchyJobRole;

            JobRoles.Add(jobRoleVM);
            _organisationDataModel.JobRoles.Add(dataModel);

            SelectedItem = jobRoleVM;
        }

        private void CreateNewEmployee(object obj)
        {
            var startingName = "Employee " + (Employees.Count + 1);
            var dataModel = new Employee() { Name = startingName, EntityKey = Guid.NewGuid() };
            var employeeVM = new EmployeeVM(_twoWayComm, dataModel, JobRoles, isNew: true);

            var startingJobRole = SelectedJobRole ?? SelectedEmployee?.JobRole;
            employeeVM.JobRole = startingJobRole;

            Employees.Add(employeeVM);
            _organisationDataModel.Employees.Add(dataModel);

            SelectedItem = employeeVM;
        }

        private void AddNewJobRoleFromServer(JobRole dataModel)
        {
            Debug.Assert(!JobRoles.Any(x => x.EntityKey == dataModel.EntityKey));

            var jobRoleVM = new JobRoleVM(_twoWayComm, dataModel, JobRoles);
            JobRoles.Add(jobRoleVM);

            _organisationDataModel.JobRoles.Add(dataModel);
        }

        private void AddNewEmployeeFromServer(Employee dataModel)
        {
            Debug.Assert(!Employees.Any(x => x.EntityKey == dataModel.EntityKey));

            var employeeVM = new EmployeeVM(_twoWayComm, dataModel, JobRoles);
            Employees.Add(employeeVM);

            _organisationDataModel.Employees.Add(dataModel);
        }

        private void RemoveLocalJob(JobRole dataModel)
        {
            var localJobRoleVM = JobRoles.FirstOrDefault(x => x.EntityKey == dataModel.EntityKey);
            if (localJobRoleVM != null)
            {
                JobRoles.Remove(localJobRoleVM);
                if (SelectedItem == localJobRoleVM)
                {
                    SelectedItem = null;
                }

                _organisationDataModel.JobRoles.Remove(dataModel);
            }
        }

        private void RemoveLocalEmployee(Employee dataModel)
        {
            var localEmployeeVM = Employees.FirstOrDefault(x => x.EntityKey == dataModel.EntityKey);
            if (localEmployeeVM != null)
            {
                Employees.Remove(localEmployeeVM);
                if (SelectedItem == localEmployeeVM)
                {
                    SelectedItem = null;
                }

                _organisationDataModel.Employees.Remove(dataModel);
            }
        }

        private void OnLoadStartingValues(Organisation newOrganisation)
        {
            _organisationDataModel.Name = newOrganisation.Name;

            foreach (var jobRole in newOrganisation.JobRoles)
            {
                AddNewJobRoleFromServer(jobRole);
            }

            foreach (var employee in newOrganisation.Employees)
            {
                AddNewEmployeeFromServer(employee);
            }

            SimpleHierarchyVM.DoAutoRefresh = true;
            SimpleHierarchyVM.RecreateHirarchy();
        }


        private void ImportNewOrganisation(Organisation newOrganisation)
        {
            SimpleHierarchyVM.DoAutoRefresh = false;
            ClearExistingData();
            _organisationDataModel.Name = newOrganisation.Name;
            _organisationDataModel.Employees.Clear();
            _organisationDataModel.JobRoles.Clear();

            foreach (var jobRole in newOrganisation.JobRoles)
            {
                _organisationDataModel.JobRoles.Add(jobRole);

                var newJobRoleVM = new JobRoleVM(_twoWayComm, jobRole, JobRoles, isNew: true);
                JobRoles.Add(newJobRoleVM);
                newJobRoleVM.SaveCommand.Execute(null);
            }

            foreach (var employee in newOrganisation.Employees)
            {
                _organisationDataModel.Employees.Add(employee);

                var newEmployeeVM = new EmployeeVM(_twoWayComm, employee, JobRoles, isNew: true);
                Employees.Add(newEmployeeVM);
                newEmployeeVM.SaveCommand.Execute(null);
            }

            SimpleHierarchyVM.DoAutoRefresh = true;
            SimpleHierarchyVM.RecreateHirarchy();
        }

        private void ClearExistingData()
        {
            foreach (var jobRole in JobRoles)
            {
                jobRole.DeleteCommand.Execute(null);
            }

            foreach (var employee in Employees)
            {
                employee.DeleteCommand.Execute(null);
            }
        }
    }
}
