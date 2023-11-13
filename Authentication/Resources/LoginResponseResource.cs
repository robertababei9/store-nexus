namespace PasswordHashExample.WebAPI.Resources;

public sealed record LoginResponseResource(
    string token, 
    bool needsToCreateCompany,
    List<string> rolePermissions
);