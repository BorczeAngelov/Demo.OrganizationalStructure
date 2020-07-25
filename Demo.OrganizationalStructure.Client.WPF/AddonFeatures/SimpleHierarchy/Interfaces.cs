using System;
using System.Collections.ObjectModel;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public interface ICompositeItem
    {
        event Action HierarchicalChange;

        Guid Key { get; }
        Guid ParentKey { get; }

        Type Type { get; } 
    }


    public interface ICompositeLeaf : ICompositeItem
    {

    }

    public interface IComposite : ICompositeItem
    {
        ObservableCollection<ICompositeItem> CompositeItems { get; }
    }
}
