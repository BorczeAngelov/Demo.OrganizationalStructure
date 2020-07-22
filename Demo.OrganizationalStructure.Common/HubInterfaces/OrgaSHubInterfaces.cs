using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Common.HubInterfaces
{
    public interface IOrgaSHub
    {

    }

    public interface IOrgaSHubClient
    {

    }

    public interface IOrgaSHubClientTwoWayComm : IOrgaSHubClient
    {
        IOrgaSHub ServerHubProxy { get; }
        Task ConnectWithServerHub();
    }
}
