using Demo.OrganizationalStructure.Common.HubInterfaces;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class OrganizationalStructureVM
    {
        private readonly IOrgaSHubClientTwoWayComm _orgaSHubClientTwoWayComm;

        internal OrganizationalStructureVM(IOrgaSHubClientTwoWayComm orgaSHubClientTwoWayComm)
        {
            _orgaSHubClientTwoWayComm = orgaSHubClientTwoWayComm;
        }
    }
}
