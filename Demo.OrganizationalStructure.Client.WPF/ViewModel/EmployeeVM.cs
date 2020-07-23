using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class EmployeeVM : EditableItemBaseVM
    {
        private Employee _dataModel;
        private string _name;

        internal EmployeeVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            Employee dataModel,
            bool isNewAndUnsaved = false)
                : base(twoWayComm, isNewAndUnsaved)
        {
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
                _dataModel = employeeFromServer;
                CopyDataFromModel();
            }
        }

        private void CopyDataFromModel()
        {
            Name = _dataModel.Name;
            // _dataModel.JobRoleEntityKey
        }

        private void CopyDataToModel()
        {
            _dataModel.Name = Name;
        }
    }
}
