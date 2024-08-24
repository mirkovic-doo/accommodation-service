using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AccommodationService.Authorization;

public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; set; }

    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new AuthorizationRequirement((AuthorizationLevel)Enum.Parse(typeof(AuthorizationLevel), policyName)));
        return Task.FromResult(policy.Build());
    }

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
}
