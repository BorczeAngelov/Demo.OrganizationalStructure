using Demo.OrganizationalStructure.Client.WPF.HubClientTwoWayComm;
using Demo.OrganizationalStructure.Common.DataModel;
using Demo.OrganizationalStructure.Common.Utils;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Demo.OrganizationalStructure.Client.MockDataGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverHubUrl = DemoUrlConstants.LocalHostUrl + DemoUrlConstants.OrgaSHubEndpoint;
            var twoWayComm = new OrgaSHubClientTwoWayComm(serverHubUrl);

            Console.WriteLine("Connecting with server...");
            await twoWayComm.ConnectWithServerHub();
            Console.WriteLine("Connection with server established.");

            var userParser = new Regex(@"(?'command'create) (?'jobRoles'\d*),(?'employees'\d*)");
            var newCommand = string.Empty;
            do
            {
                Console.WriteLine("Syntax \"create x,y\", where x is number of new jobs and y is number of new employees to be added for each new job.\n");
                newCommand = Console.ReadLine().ToLower();

                if (userParser.IsMatch(newCommand))
                {
                    var match = userParser.Match(newCommand);
                    var newJobRoles = int.Parse(match.Groups["jobRoles"].Value);
                    var newEmployees = int.Parse(match.Groups["employees"].Value);

                    for (int i = 0; i < newJobRoles; i++)
                    {
                        var jobRoleMock = new JobRole() { Name = "Job role mock " + (i + 1), EntityKey = Guid.NewGuid() };
                        await twoWayComm.ServerHubProxy.CreateJobRole(jobRoleMock);

                        for (int j = 0; j < newEmployees; j++)
                        {
                            var employeeMock = new Employee() { Name = "Employee mock " + (j + 1), EntityKey = Guid.NewGuid(), JobRoleEntityKey = jobRoleMock.EntityKey };
                            await twoWayComm.ServerHubProxy.CreateEmployee(employeeMock);
                        }
                    }

                    Console.WriteLine("Success.");
                }
                else
                {
                    Console.WriteLine("Invalid command. Please try again, or write -1 to exit.");
                }

            } while (newCommand != "-1");
        }
    }
}
