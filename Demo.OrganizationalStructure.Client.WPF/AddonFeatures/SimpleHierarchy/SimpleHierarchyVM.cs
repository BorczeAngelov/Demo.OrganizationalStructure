
using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public class SimpleHierarchyVM
    {
        private readonly OrganizationalStructureVM _organizationalStructureVM;

        internal SimpleHierarchyVM(OrganizationalStructureVM organizationalStructureVM)
        {
            _organizationalStructureVM = organizationalStructureVM;

            _organizationalStructureVM.JobRoles.CollectionChanged += (s, e) => RecreateHirarchy();
            _organizationalStructureVM.Employees.CollectionChanged += (s, e) => RecreateHirarchy();
        }

        public ObservableCollection<ICompositeItem> HirarchyItems { get; } = new ObservableCollection<ICompositeItem>();

        // Note: Simple, but resources intensiv solution        
        private void RecreateHirarchy()
        {
            HirarchyItems.Clear();

            var composites = _organizationalStructureVM.JobRoles.Select(
                x =>
                {
                    var item = new CompositeJobRoleVM(x);
                    item.HierarchicalChange += () => RecreateHirarchy();
                    return item;
                }).ToList();

            var compositeLeafs = _organizationalStructureVM.Employees.Select(
                x =>
                {
                    var item = new CompositeLeafEmployeeVM(x);
                    item.HierarchicalChange += () => RecreateHirarchy();
                    return item;
                }).ToList();

            foreach (var compositeItem in composites)
            {
                AddItemToHirarchy(compositeItem, composites);
            }

            foreach (var compositeItem in compositeLeafs)
            {
                AddItemToHirarchy(compositeItem, composites);
            }
        }

        private void AddItemToHirarchy(
            ICompositeItem compositeItem,
            IEnumerable<IComposite> composites)
        {
            var parentComposite = composites.FirstOrDefault(x => x.Key == compositeItem.ParentKey);
            if (parentComposite != null)
            {
                parentComposite.CompositeItems.Add(compositeItem);
            }
            else
            {
                HirarchyItems.Add(compositeItem);
            }
        }
    }
}
