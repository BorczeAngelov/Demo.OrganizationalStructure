using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public abstract class EditableItemBaseVM : ObservableBase
    {
        protected readonly IOrgaSHubClientTwoWayComm TwoWayComm;
        private bool _isNewAndUnsaved;

        protected EditableItemBaseVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            bool isNewAndUnsaved)
        {
            TwoWayComm = twoWayComm;
            IsNewAndUnsaved = isNewAndUnsaved;

            SaveCommand = new DelegateCommand(Save);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get /*=> throw new NotImplementedException()*/; }
        public DelegateCommand DeleteCommand { get /*=> throw new NotImplementedException()*/; }

        public bool IsNewAndUnsaved
        {
            get => _isNewAndUnsaved;
            private set
            {
                if (_isNewAndUnsaved != value)
                {
                    _isNewAndUnsaved = value;
                    OnPropertyChanged();
                }
            }
        }

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
