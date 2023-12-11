using PasswordHashExample.WebAPI.Resources;
using Authentication.Services;
using MediatR;
using Common.Models;
using Infrastructure.Repositories.Contracts;
using Application.ExecutionHelper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Commands.Users
{
    public static class RegisterFromInvitee
    {
        // Command
        public record Command(RegisterFromInviteeResource registerModel) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Command, Response>
        {
            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            protected IUserInvitationsRepository _userInvitationsRepository { get; set; }

            private readonly ILogger<Handler> _logger;
            private readonly IUserService _userService;

            public Handler(ILogger<Handler> logger,
                IHttpContextAccessor httpContextAccessor,
                IUserInvitationsRepository userInvitationsRepository,
                IUserService userService)
            {
                _logger = logger;
                _httpContextAccessor = httpContextAccessor;
                _userService = userService;
                _userInvitationsRepository = userInvitationsRepository;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                return await ExecuteFunc.TryExecute<Task<Response>>(async () =>
                {
                    _logger.LogInformation("RegisterFromInvitee -> Started execution for RegisterFromInvitee() started");
                    var response = new ApiResponseModel<bool>();

                    // we will get the token so we know who invited the user
                    // we will also verify in the DB that the token really invited this user
                    string token = "";
                    string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                    if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                    {
                        token = authorizationHeader.Substring("Bearer ".Length).Trim();
                    }

                    if (string.IsNullOrEmpty(token))
                    {
                        _logger.LogInformation("RegisterFromInvitee -> Couldn't get the token from request");
                        response.Success = false;
                        return new Response(response);
                        //throw new Exception("Could not get retrieve the token");
                    }

                    string inviterId = new JwtSecurityToken(token).Claims.FirstOrDefault(x => x.Type == "Id").Value;
                    string companyId = new JwtSecurityToken(token).Claims.FirstOrDefault(x => x.Type == "CompanyId").Value;
                    
                    // Check the invitation ( also check the validity ---- < 24h )
                    var invitedUser = await _userInvitationsRepository
                        .FirstOrDefaultAsync(x =>
                            x.InviterId == Guid.Parse(inviterId) &&
                            x.Email == request.registerModel.Email &&
                            x.Created == false, x => x);

                    if (invitedUser == null)
                    {
                        _logger.LogInformation("RegisterFromInvitee -> Error while trying to create the invited user");
                        response.Success = false;
                        response.Errors.Add("Something went wrong trying to create the user");
                        return new Response(response);
                    }

                    var hoursPassed = (DateTime.Now - invitedUser.CreatedAt).TotalHours;
                    if (hoursPassed >= 24)
                    {
                        _logger.LogInformation("RegisterFromInvitee -> User invitation expired");
                        response.Success = false;
                        response.Errors.Add("Invitation expired");
                        return new Response(response);
                    }

                    // create the user
                    var user = await _userService.Register(new RegisterResource(
                                request.registerModel.FirstName,
                                request.registerModel.LastName,
                                request.registerModel.Email,
                                request.registerModel.Password,
                                invitedUser.RoleId,
                                Guid.Parse(companyId)
                    ), CancellationToken.None);

                    invitedUser.Created = true;
                    _userInvitationsRepository.Update(invitedUser);
                    await _userInvitationsRepository.SaveChangesAsync();


                    _logger.LogInformation("RegisterFromInvitee -> Finished execution for RegisterFromInvitee()");
                    return new Response(response);
                }, _logger);

            }

        }

        // Response
        public record Response(ApiResponseModel<bool> response);
    }
}
