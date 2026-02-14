using Microsoft.AspNetCore.Authorization;

namespace salian_api.Config.Permissions
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        internal const string PolicyPrefix = "PERMISSION:";

        // create our permissions with special format to find them next and check them
        public PermissionAuthorizeAttribute(params string[] permissions)
        {
            Policy = $"{PolicyPrefix}{string.Join(",", permissions)}";
        }
    }
}