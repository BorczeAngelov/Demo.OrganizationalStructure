using System.Windows;
using System.Windows.Controls;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public partial class SimpleHierarchyView : UserControl
    {
        private SimpleHierarchyVM _simpleHierarchyVM;

        public SimpleHierarchyView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _simpleHierarchyVM = (SimpleHierarchyVM)DataContext;
            _simpleHierarchyVM.SelectedCompositeItemChanged += SelectItemFromVM;
        }

        private void SelectItemInVM(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _simpleHierarchyVM.SelectItem(e.NewValue as ICompositeItem);
        }

        private void SelectItemFromVM(ICompositeItem obj)
        {
            if (FindTreeViewItem(TreeViewControl, obj) is TreeViewItem treeViewItem)
            {
                treeViewItem.IsSelected = true;
            }
        }

        //Note: https://stackoverflow.com/a/32628562
        private TreeViewItem FindTreeViewItem(
            ItemsControl itemsControl,
            ICompositeItem findItem)
        {
            //Search for the object model in first level children (recursively)
            if (itemsControl.ItemContainerGenerator.ContainerFromItem(findItem) is TreeViewItem positiveResult)
            {
                return positiveResult;
            }

            //Loop through user object models
            foreach (var item in itemsControl.Items)
            {
                //Get the TreeViewItem associated with the iterated object model
                if (itemsControl.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem tviLevel2 &&
                    FindTreeViewItem(tviLevel2, findItem) is TreeViewItem positiveResultLevel2)
                {
                    return positiveResultLevel2;
                }
            }
            return null;
        }
    }
}
