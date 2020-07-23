using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class JobRoleVM : EditableItemBaseVM
    {
        private readonly JobRole _dataModel;
        private string _name;

        internal JobRoleVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            JobRole dataModel,
            bool isNewAndUnsaved = false)
                : base(twoWayComm, isNewAndUnsaved)
        {
            _dataModel = dataModel;
            CopyDataFromModel(dataModel);
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
            TwoWayComm.ServerHubProxy.CreateJobRole(_dataModel);
        }

        protected override void Save()
        {
            TwoWayComm.ServerHubProxy.UpdateJobRole(_dataModel);
        }

        protected override void DiscardChanges()
        {
            CopyDataFromModel(_dataModel);
        }

        protected override void Delete()
        {
            TwoWayComm.ServerHubProxy.DeleteJobRole(_dataModel);
        }

        private void CopyDataFromModel(JobRole model)
        {
            Name = model.Name;
        }
    }
}
