using System.Collections.ObjectModel;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public interface ICompositeItem
    {
    }

    public interface ICompositeLeaf : ICompositeItem
    {

    }

    public interface IComposite : ICompositeItem
    {
        ObservableCollection<ICompositeItem> CompositeItems { get; }
    }
}
