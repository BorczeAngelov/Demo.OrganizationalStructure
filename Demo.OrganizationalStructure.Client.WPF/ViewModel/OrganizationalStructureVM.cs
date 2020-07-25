﻿using Demo.OrganizationalStructure.Client.WPF.AddonFeatures.ImportExport;
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

            ExportCommand = new DelegateCommand(arg => _importExportImp.Export(_organisationDataModel));

            AddJobRoleCommand = new DelegateCommand(CreateNewJobRole);
            AddEmployeeCommand = new DelegateCommand(CreateNewEmployee);

            _twoWayComm = twoWayComm;
            _twoWayComm.JobRoleCreated += AddNewJobRoleFromServer;
            _twoWayComm.EmployeeCreated += AddNewEmployeeFromServer;

            _twoWayComm.JobRoleDeleted += RemoveLocalJob;
            _twoWayComm.EmployeeDeleted += RemoveLocalEmployee;

            _twoWayComm.LoadStartingValues += OnLoadStartingValues;
        }

        public SimpleHierarchyVM SimpleHierarchyVM { get; }
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
            var jobRoleVM = new JobRoleVM(_twoWayComm, dataModel, JobRoles, isNewAndUnsaved: true);
            JobRoles.Add(jobRoleVM);

            SelectedItem = jobRoleVM;

            _organisationDataModel.JobRoles.Add(dataModel);
        }

        private void CreateNewEmployee(object obj)
        {
            var startingName = "Employee " + (Employees.Count + 1);
            var dataModel = new Employee() { Name = startingName, EntityKey = Guid.NewGuid() };
            var employeeVM = new EmployeeVM(_twoWayComm, dataModel, JobRoles, isNewAndUnsaved: true);
            Employees.Add(employeeVM);

            SelectedItem = employeeVM;

            _organisationDataModel.Employees.Add(dataModel);
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

        private void OnLoadStartingValues(Organisation organisation)
        {
            _organisationDataModel.Name = organisation.Name;

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
