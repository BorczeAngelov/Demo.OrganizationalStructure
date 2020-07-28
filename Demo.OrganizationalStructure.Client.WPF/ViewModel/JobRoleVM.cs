using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class JobRoleVM : EditableItemBaseVM
    {
        private JobRole _dataModel;
        private string _name;
        private string _description;
        private JobRoleVM _upperHierarchyJobRole;
        private string _exceptionOfUpperHierarchyJobRole;

        internal JobRoleVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            JobRole dataModel,
            ObservableCollection<JobRoleVM> existingJobRoleVMs,
            bool isNew = false)
                : base(twoWayComm, isNew)
        {
            ExistingJobRoleVMs = existingJobRoleVMs;

            _dataModel = dataModel;
            CopyDataFromModel();

            TwoWayComm.JobRoleUpdated += UpdateDataIfIsSameEntity;
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

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<JobRoleVM> ExistingJobRoleVMs { get; }
        public JobRoleVM UpperHierarchyJobRole
        {
            get => _upperHierarchyJobRole;
            set
            {
                if (_upperHierarchyJobRole != value)
                {
                    if (!ValidateNewUpperHierarchyValue(value, out InvalidUpperHierarchyValueException exception))
                    {
                        ErrorMessageOfUpperHierarchyJobRole = exception.Message;
                        throw exception;
                    }
                    ErrorMessageOfUpperHierarchyJobRole = null;

                    _upperHierarchyJobRole = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorMessageOfUpperHierarchyJobRole
        {
            get => _exceptionOfUpperHierarchyJobRole;
            private set
            {
                if (_exceptionOfUpperHierarchyJobRole != value)
                {
                    _exceptionOfUpperHierarchyJobRole = value;
                    OnPropertyChanged();
                }
            }
        }

        internal void ClearExceptionMessage()
        {
            ErrorMessageOfUpperHierarchyJobRole = null;
        }

        protected override void SaveNew()
        {
            CopyDataToModel();
            TwoWayComm.ServerHubProxy.CreateJobRole(_dataModel);
        }

        protected override void Save()
        {
            CopyDataToModel();
            TwoWayComm.ServerHubProxy.UpdateJobRole(_dataModel);
        }

        protected override void DiscardChanges()
        {
            CopyDataFromModel();
        }

        protected override void Delete()
        {
            TwoWayComm.ServerHubProxy.DeleteJobRole(_dataModel);
        }

        private void UpdateDataIfIsSameEntity(JobRole jobRoleFromServer)
        {
            var isSameEntity = _dataModel.EntityKey == jobRoleFromServer.EntityKey;
            if (isSameEntity)
            {
                _dataModel.Name = jobRoleFromServer.Name;
                _dataModel.Description = jobRoleFromServer.Description;
                _dataModel.UpperHierarchyJobRoleKey = jobRoleFromServer.UpperHierarchyJobRoleKey;
                CopyDataFromModel();
            }
        }

        private void CopyDataFromModel()
        {
            IsLoadingDataFromModel = true;
            Name = _dataModel.Name;
            Description = _dataModel.Description;
            LoadUpperHierarchyJobRoleFromExistingVMs();
            IsLoadingDataFromModel = false;
        }

        internal void LoadUpperHierarchyJobRoleFromExistingVMs()
        {
            IsLoadingDataFromModel = true;
            UpperHierarchyJobRole = ExistingJobRoleVMs.FirstOrDefault(x => x.EntityKey == _dataModel.UpperHierarchyJobRoleKey);
            IsLoadingDataFromModel = false;
        }

        private void CopyDataToModel()
        {
            _dataModel.Name = Name;
            _dataModel.Description = Description;
            _dataModel.UpperHierarchyJobRoleKey = (UpperHierarchyJobRole != null) ? UpperHierarchyJobRole.EntityKey : Guid.Empty;
        }

        private bool ValidateNewUpperHierarchyValue(
            JobRoleVM value,
            out InvalidUpperHierarchyValueException exception)
        {
            if (value == this)
            {
                exception = new InvalidUpperHierarchyValueException("Cannot set itself as upper hierarchy.");
                return false;
            }

            var isRootFound = SearchForRoot(value);
            if (!isRootFound)
            {
                exception = new InvalidUpperHierarchyValueException("Hierarchy cannot be set.");
                return false;
            }

            exception = null;
            return true;
        }

        private bool SearchForRoot(JobRoleVM value)
        {
            var isRootFound = value?.UpperHierarchyJobRole is null;
            if (!isRootFound)
            {
                var stackOverflow = value.UpperHierarchyJobRole == this;
                if (!stackOverflow)
                {
                    return SearchForRoot(value.UpperHierarchyJobRole);
                }
                else
                {
                    return false;
                }
            }
            return isRootFound;
        }
    }
}
