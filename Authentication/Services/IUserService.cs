using PasswordHashExample.WebAPI.Resources;


namespace Authentication.Services
{
    public interface IUserService
    {
        Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken);
        Task<string> Login(LoginResource resource, CancellationToken cancellationToken);
    }
}
