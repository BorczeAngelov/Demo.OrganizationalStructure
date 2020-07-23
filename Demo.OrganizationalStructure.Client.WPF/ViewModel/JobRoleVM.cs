using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.HubInterfaces;

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
            TwoWayComm.ServerHubProxy.CreateJobRole(_dataModel);
        }

        protected override void Save()
        {
            TwoWayComm.ServerHubProxy.UpdateJobRole(_dataModel);
        }
    }
}
