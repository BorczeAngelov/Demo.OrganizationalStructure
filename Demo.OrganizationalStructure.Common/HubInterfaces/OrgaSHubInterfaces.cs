using System;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Common.HubInterfaces
{
    public interface IOrgaSHub
    {
        Task PingDemo();
    }

    public interface IOrgaSHubClient
    {
        void InvokePongDemo();
    }

    public interface IOrgaSHubClientTwoWayComm : IOrgaSHubClient
    {
        event Action PongedDemo;

        IOrgaSHub ServerHubProxy { get; }
        Task ConnectWithServerHub();
    }
}
