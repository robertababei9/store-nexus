namespace PasswordHashExample.WebAPI.Resources;

public sealed record ExistingPasswordResource(string Password, string PasswordSalt);
