using AccommodationService.Contracts.Constants;
using AccommodationService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AccommodationService.Authorization;

public class AuthorizationLevelAuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
{
    private readonly AccommodationDbContext dbContext;

    public AuthorizationLevelAuthorizationHandler(AccommodationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
    {
        if (context.User == null)
        {
            context.Fail();
            return;
        }

        var claim = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.IsGuest);
        if (claim != null)
        {
            AuthorizeUser(context, requirement, claim);
            return;
        }

        context.Fail();
    }

    private void AuthorizeUser(AuthorizationHandlerContext context, AuthorizationRequirement requirement, Claim claim)
    {
        var isGuestClaimValue = claim.Value;

        if (!bool.TryParse(isGuestClaimValue, out var isGuest))
        {
            context.Fail();
            return;
        }

        if (isGuest && requirement.AuthorizationLevel == AuthorizationLevel.Guest || !isGuest && requirement.AuthorizationLevel == AuthorizationLevel.Host)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}
