using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System.Collections.ObjectModel;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public class CompositeJobRoleVM : IComposite
    {
        internal CompositeJobRoleVM(JobRoleVM jobRoleVM)
        {
            JobRoleVM = jobRoleVM;
        }

        public JobRoleVM JobRoleVM { get; }
        public ObservableCollection<ICompositeItem> CompositeItems { get; }

    }
}
