using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class JobRoleVM : EditableItemBaseVM
    {
        private JobRole _dataModel;
        private string _name;

        internal JobRoleVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            JobRole dataModel,
            bool isNewAndUnsaved = false)
                : base(twoWayComm, isNewAndUnsaved)
        {
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
                _dataModel = jobRoleFromServer;
                CopyDataFromModel();
            }
        }

        private void CopyDataFromModel()
        {
            Name = _dataModel.Name;
            // _dataModel.Description
        }

        private void CopyDataToModel()
        {
            _dataModel.Name = Name;
        }
    }
}
