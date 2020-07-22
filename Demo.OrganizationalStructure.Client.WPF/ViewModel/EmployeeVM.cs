using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.DataModel;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class EmployeeVM : ObservableBase
    {
        private readonly Employee _dataModel;

        internal EmployeeVM(Employee dataModel)
        {
            _dataModel = dataModel;
        }
    }
}
