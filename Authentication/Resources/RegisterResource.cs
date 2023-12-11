namespace PasswordHashExample.WebAPI.Resources;

public sealed record RegisterResource(
    string FirstName,
    string LastName,
    string Email, 
    string Password, 
    Guid RoleId,
    Guid? CompanyId
);

public sealed record RegisterFromInviteeResource(
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Password
);

