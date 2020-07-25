using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System;
using System.ComponentModel;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public class CompositeLeafEmployeeVM : ICompositeLeaf
    {
        public event Action HierarchicalChange;

        internal CompositeLeafEmployeeVM(EmployeeVM employeeVM)
        {
            EmployeeVM = employeeVM;
            EmployeeVM.PropertyChanged += OnJobRoleChanged;
        }

        public EmployeeVM EmployeeVM { get; }

        public Guid Key { get => EmployeeVM.EntityKey; }
        public Guid ParentKey { get => (EmployeeVM.JobRole != null) ? EmployeeVM.JobRole.EntityKey : Guid.Empty; }

        private void OnJobRoleChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmployeeVM.JobRole))
            {
                HierarchicalChange?.Invoke();
            }
        }
    }
}
