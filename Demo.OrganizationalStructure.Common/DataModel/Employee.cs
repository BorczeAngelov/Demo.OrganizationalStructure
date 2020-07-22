using System;

namespace Demo.OrganizationalStructure.Common.DataModel
{
    public class Employee
    {
        public Guid EntityKey { get; set; }
        public string Name { get; set; }
        public Guid JobRoleEntityKey { get; set; }
    }
}
