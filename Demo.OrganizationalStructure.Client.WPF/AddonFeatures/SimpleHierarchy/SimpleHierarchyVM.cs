using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System;
using System.Collections.Specialized;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public class SimpleHierarchyVM
    {
        private readonly OrganizationalStructureVM _organizationalStructureVM;

        internal SimpleHierarchyVM(OrganizationalStructureVM organizationalStructureVM)
        {
            _organizationalStructureVM = organizationalStructureVM;

            _organizationalStructureVM.JobRoles.CollectionChanged += SyncWithJobRoles;
            _organizationalStructureVM.Employees.CollectionChanged += SyncWithEmployees;
        }

        private void SyncWithJobRoles(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var jobRoleVM = (JobRoleVM)item;
                    var compositeJobRoleVM = new CompositeJobRoleVM(jobRoleVM);

                    //1: Search upwards & Inject itself                    
                    //2: Observe when UpperHierarchy changes, and update it self
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var jobRoleVM = (JobRoleVM)item;
                    //var compositeJobRoleVM = new CompositeJobRoleVM(jobRoleVM);

                    //TODO: Search composite & remove itself                    
                }
            }
        }


        private void SyncWithEmployees(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
