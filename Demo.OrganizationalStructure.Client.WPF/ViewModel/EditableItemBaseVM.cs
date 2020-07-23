﻿using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.HubInterfaces;
using System;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public abstract class EditableItemBaseVM : ObservableBase
    {
        protected readonly IOrgaSHubClientTwoWayComm TwoWayComm;
        private bool _isNewAndUnsaved;
        private bool _isModified;

        protected abstract void Save();
        protected abstract void SaveNew();
        protected abstract void DiscardChanges();
        protected abstract void Delete();

        protected EditableItemBaseVM(
            IOrgaSHubClientTwoWayComm twoWayComm,
            bool isNewAndUnsaved)
        {
            TwoWayComm = twoWayComm;
            IsNewAndUnsaved = isNewAndUnsaved;

            SaveCommand = new DelegateCommand(Save, arg => IsNewAndUnsaved || IsModified);
            CancelCommand = new DelegateCommand(Cancel, arg => IsModified);
            DeleteCommand = new DelegateCommand(Delete);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand DeleteCommand { get; }

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
            if (!IsNewAndUnsaved)
            {
                Save();
            }
            else
            {
                SaveNew();
                IsNewAndUnsaved = false;
            }
        }

        private void Cancel(object obj)
        {
            DiscardChanges();
        }

        private void Delete(object obj)
        {
            Delete();
        }
    }
}
