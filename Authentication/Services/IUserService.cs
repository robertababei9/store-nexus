using PasswordHashExample.WebAPI.Resources;


namespace Authentication.Services
{
    public interface IUserService
    {
        Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken);
        Task<UserResource> Login(LoginResource resource, CancellationToken cancellationToken);
    }
}
