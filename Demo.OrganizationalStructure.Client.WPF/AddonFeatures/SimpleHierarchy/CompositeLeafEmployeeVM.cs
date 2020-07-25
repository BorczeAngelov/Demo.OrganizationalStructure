using Demo.OrganizationalStructure.Client.WPF.ViewModel;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public class CompositeLeafEmployeeVM : ICompositeLeaf
    {
        internal CompositeLeafEmployeeVM(EmployeeVM employeeVM)
        {
            EmployeeVM = employeeVM;
        }

        public EmployeeVM EmployeeVM { get; }
    }
}
