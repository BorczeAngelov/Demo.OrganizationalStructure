using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.HubInterfaces;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public abstract class EditableItemBaseVM : ObservableBase
    {
        protected readonly IOrgaSHubClientTwoWayComm TwoWayComm;

        protected EditableItemBaseVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            bool isNewAndUnsaved)
        {
            TwoWayComm = twoWayComm;
            IsNewAndUnsaved = isNewAndUnsaved;

            SaveCommand = new DelegateCommand(Save);
        }

        public DelegateCommand SaveCommand { get; }
        public bool IsNewAndUnsaved { get; private set; }

        protected abstract void SaveNew();
        protected abstract void Save();

        private void Save(object obj)
        {
            if (IsNewAndUnsaved)
            {
                SaveNew();
                IsNewAndUnsaved = false;
            }
            else
            {
                Save();
            }
        }
    }
}
