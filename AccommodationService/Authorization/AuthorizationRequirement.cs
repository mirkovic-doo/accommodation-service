using Microsoft.AspNetCore.Authorization;

namespace AccommodationService.Authorization;

public class AuthorizationRequirement : IAuthorizationRequirement
{
    public AuthorizationLevel AuthorizationLevel { get; set; }

    public AuthorizationRequirement(AuthorizationLevel permission)
    {
        AuthorizationLevel = permission;
    }
}
