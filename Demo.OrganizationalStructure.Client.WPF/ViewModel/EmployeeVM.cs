using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class EmployeeVM : EditableItemBaseVM
    {
        private Employee _dataModel
            ;
        private string _name;
        private JobRoleVM _jobRole;

        internal EmployeeVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            Employee dataModel,
            ObservableCollection<JobRoleVM> existingJobRoleVMs,
            bool isNew = false)
                : base(twoWayComm, isNew)
        {
            ExistingJobRoleVMs = existingJobRoleVMs;

            _dataModel = dataModel;
            CopyDataFromModel();

            TwoWayComm.EmployeeUpdated += UpdateDataIfIsSameEntity;
        }

        public Guid EntityKey { get => _dataModel.EntityKey; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<JobRoleVM> ExistingJobRoleVMs { get; }
        public JobRoleVM JobRole
        {
            get => _jobRole;
            set
            {
                if (_jobRole != value)
                {
                    _jobRole = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void SaveNew()
        {
            CopyDataToModel();
            TwoWayComm.ServerHubProxy.CreateEmployee(_dataModel);
        }

        protected override void Save()
        {
            CopyDataToModel();
            TwoWayComm.ServerHubProxy.UpdateEmployee(_dataModel);
        }

        protected override void DiscardChanges()
        {
            CopyDataFromModel();
        }

        protected override void Delete()
        {
            TwoWayComm.ServerHubProxy.DeleteEmployee(_dataModel);
        }

        private void UpdateDataIfIsSameEntity(Employee employeeFromServer)
        {
            var isSameEntity = _dataModel.EntityKey == employeeFromServer.EntityKey;
            if (isSameEntity)
            {
                _dataModel.Name = employeeFromServer.Name;
                _dataModel.JobRoleEntityKey = employeeFromServer.JobRoleEntityKey;
                CopyDataFromModel();
            }
        }

        private void CopyDataFromModel()
        {
            Name = _dataModel.Name;
            JobRole = ExistingJobRoleVMs.FirstOrDefault(x => x.EntityKey == _dataModel.JobRoleEntityKey);
        }

        private void CopyDataToModel()
        {
            _dataModel.Name = Name;
            _dataModel.JobRoleEntityKey = (JobRole != null) ? JobRole.EntityKey : Guid.Empty;
        }
    }
}
