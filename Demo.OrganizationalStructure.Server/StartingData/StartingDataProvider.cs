using Demo.OrganizationalStructure.Common.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Demo.OrganizationalStructure.Server.StartingData
{
    internal static class StartingDataProvider
    {
        internal static Organisation GetOrganisation()
        {
            var organisation = new Organisation();
            try
            {
                var jsonValue = File.ReadAllText(@".\StartingData\StartingDataMock.json");
                organisation = JsonConvert.DeserializeObject<Organisation>(jsonValue);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }

            organisation.Employees ??= new List<Employee>();
            organisation.JobRoles ??= new List<JobRole>();
            return organisation;
        }
    }
}
