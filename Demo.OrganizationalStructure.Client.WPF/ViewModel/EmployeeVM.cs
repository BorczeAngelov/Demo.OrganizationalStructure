using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class EmployeeVM : EditableItemBaseVM
    {
        private readonly Employee _dataModel;
        private string _name;

        internal EmployeeVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            Employee dataModel,
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
            CopyDataFromModel(_dataModel);
        }

        protected override void Delete()
        {
            TwoWayComm.ServerHubProxy.DeleteEmployee(_dataModel);
        }

        private void CopyDataFromModel(Employee model)
        {
            Name = model.Name;
        }
        private void CopyDataToModel()
        {
            _dataModel.Name = Name;
        }
    }
}
