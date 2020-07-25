using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public class CompositeJobRoleVM : IComposite
    {
        public event Action HierarchicalChange;

        internal CompositeJobRoleVM(JobRoleVM jobRoleVM)
        {
            JobRoleVM = jobRoleVM;
            JobRoleVM.PropertyChanged += OnUpperHierarchyJobRoleChanged;
        }

        public JobRoleVM JobRoleVM { get; }
        public ObservableCollection<ICompositeItem> CompositeItems { get; } = new ObservableCollection<ICompositeItem>();

        public Type Type { get => GetType(); }
        public Guid Key { get => JobRoleVM.EntityKey; }
        public Guid ParentKey { get => (JobRoleVM.UpperHierarchyJobRole != null) ? JobRoleVM.UpperHierarchyJobRole.EntityKey : Guid.Empty; }


        private void OnUpperHierarchyJobRoleChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(JobRoleVM.UpperHierarchyJobRole))
            {
                HierarchicalChange?.Invoke();
            }
        }
    }
}
