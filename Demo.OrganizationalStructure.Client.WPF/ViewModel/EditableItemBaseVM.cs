using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;
using System.ComponentModel;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public abstract class EditableItemBaseVM : ObservableBase
    {
        protected readonly IOrgaSHubClientTwoWayComm TwoWayComm;
        private bool _isNew;
        private bool _isModified;

        protected abstract void Save();
        protected abstract void SaveNew();
        protected abstract void DiscardChanges();
        protected abstract void Delete();

        protected EditableItemBaseVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            bool isNew)
        {
            TwoWayComm = twoWayComm;
            IsNew = isNew;

            SaveCommand = new DelegateCommand(Save, arg => IsNew || IsModified);
            CancelCommand = new DelegateCommand(Cancel, arg => !IsNew && IsModified);
            DeleteCommand = new DelegateCommand(Delete);

            PropertyChanged += WhenAnyPropertyIsChangedMarkAsModified;
        }

        public Type VmType { get => GetType(); }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        protected bool IsLoadingDataFromModel { get; set; }

        public bool IsNew
        {
            get => _isNew;
            private set
            {
                if (_isNew != value)
                {
                    _isNew = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsModified
        {
            get => _isModified;
            protected set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    OnPropertyChanged();
                    CancelCommand.RaiseCanExecuteChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void Save(object obj)
        {
            if (!IsNew)
            {
                Save();
            }
            else
            {
                SaveNew();
                IsNew = false;
            }
            IsModified = false;
        }

        private void Cancel(object obj)
        {
            DiscardChanges();
        }

        private void Delete(object obj)
        {
            Delete();
        }

        private void WhenAnyPropertyIsChangedMarkAsModified(object sender, PropertyChangedEventArgs e)
        {
            var quickWorkaround = e.PropertyName != nameof(JobRoleVM.ErrorMessageOfUpperHierarchyJobRole); //Note: this should be refactored

            if (e.PropertyName != nameof(IsModified) &&
                !IsLoadingDataFromModel &&
                quickWorkaround)
            {
                IsModified = true;
            }
        }
    }
}
