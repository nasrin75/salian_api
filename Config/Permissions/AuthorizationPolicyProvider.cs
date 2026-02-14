
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace salian_api.Config.Permissions
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // check that policies are't in our policy that we created( for permission ) use base asp.net policy
            if (!policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return base.GetPolicyAsync(policyName);
            }

            var permissionNames = policyName.Substring(PermissionAuthorizeAttribute.PolicyPrefix.Length).Split(',');

            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim(Permissions.Permission, permissionNames)
                .Build();

            return Task.FromResult(policy);
        }
    }
}