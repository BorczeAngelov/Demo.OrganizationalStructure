using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.DataModel;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class JobRoleVM : ObservableBase
    {
        private readonly JobRole _dataModel;

        internal JobRoleVM(JobRole dataModel)
        {
            _dataModel = dataModel;
        }
    }
}
