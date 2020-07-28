using System;

namespace Demo.OrganizationalStructure.Client.WPF.Utils
{
    public class InvalidUpperHierarchyValueException : Exception
    {
        public InvalidUpperHierarchyValueException(string message) : base(message)
        {
        }

        public override string Message { get => base.Message; }
    }
}
