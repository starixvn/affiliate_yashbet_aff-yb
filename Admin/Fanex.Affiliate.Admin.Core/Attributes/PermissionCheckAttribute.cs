using System;

namespace Fanex.Affiliate.Admin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class PermissionCheckAttribute : Attribute
    {
        public string PermissionName { get; }

        public PermissionCheckAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
