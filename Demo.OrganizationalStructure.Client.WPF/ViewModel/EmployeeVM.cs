using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;

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
            Name = _dataModel.Name;
        }

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
            TwoWayComm.ServerHubProxy.CreateEmployee(_dataModel);
        }

        protected override void Save()
        {
            TwoWayComm.ServerHubProxy.UpdateEmployee(_dataModel);
        }
    }
}
