using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.ChangesLiveUpload
{
    public class ChangesLiveUploadVM : ObservableBase
    {
        private readonly OrganizationalStructureVM _organizationalStructureVM;
        private bool _shouldDoLiveUploads;

        public ChangesLiveUploadVM(OrganizationalStructureVM organizationalStructureVM)
        {
            _organizationalStructureVM = organizationalStructureVM;

            _organizationalStructureVM.JobRoles.CollectionChanged += WhenItemIsModifiedInvokeSave;
            _organizationalStructureVM.Employees.CollectionChanged += WhenItemIsModifiedInvokeSave;
        }

        public bool ShouldDoLiveUploads
        {
            get => _shouldDoLiveUploads;
            set
            {
                if (_shouldDoLiveUploads != value)
                {
                    _shouldDoLiveUploads = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShouldDoTransactionUploads));
                }
            }
        }

        public bool ShouldDoTransactionUploads
        {
            get => !ShouldDoLiveUploads;
            set => ShouldDoLiveUploads = !value;
        }

        private void WhenItemIsModifiedInvokeSave(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (EditableItemBaseVM item in e.NewItems)
                {
                    item.PropertyChanged += WhenModifiedInvokeSave;
                }
            }
        }

        private void WhenModifiedInvokeSave(object sender, PropertyChangedEventArgs e)
        {
            if (ShouldDoLiveUploads &&
                sender is EditableItemBaseVM editableItemBase &&
                editableItemBase.IsModified)
            {
                editableItemBase.SaveCommand.Execute(null);
            }
        }
    }
}
