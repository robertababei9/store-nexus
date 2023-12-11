using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;
using Microsoft.AspNetCore.Http;
using Domain.Dto.Users;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Email;
using MimeKit;
using Application.ExecutionHelper;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Application.ExecutionHelper.Exceptions;
using Common.Constants;

namespace Application.Commands.Users
{
    public static class InviteUser
    {
        // Command
        public record Command(InviteUserDto userToInvite) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Command, Response>
        {

            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            protected IUserRepository _userRepository { get; set; }
            protected IRolesRepository _rolesRepository { get; set; }
            protected IUserInvitationsRepository _userInvitationsRepository { get; set; }

            private readonly ILogger<Handler> _logger;
            private readonly IMailService _mailService;


            public Handler(
                ILogger<Handler> logger,
                IHttpContextAccessor httpContextAccessor,
                IUserRepository userRepository,
                IRolesRepository rolesRepository,
                IMailService mailService,
                IUserInvitationsRepository userInvitationsRepository
                )
            {
                _logger = logger;
                _httpContextAccessor = httpContextAccessor;
                _userRepository = userRepository;
                _rolesRepository = rolesRepository;
                _mailService = mailService;
                _userInvitationsRepository = userInvitationsRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                return await ExecuteFunc.TryExecute<Task<Response>>(async () =>
                {
                    _logger.LogInformation("InviteUser -> Started execution for InviteUser started");
                    var response = new ApiResponseModel<bool>();

                    // check if another email already exist
                    bool emailAlreadyExist = (await _userRepository.FirstOrDefaultAsync(x => x.Email == request.userToInvite.Email, x => x)) != null;
                    if (emailAlreadyExist)
                    {
                        response.Success = false;
                        response.Errors.Add("Email address is already in use");
                        return new Response(response);
                    }

                    // check if invitation was already sent in <24h
                    var invitedUser = await _userInvitationsRepository
                        .FirstOrDefaultAsync(x =>
                            x.Email == request.userToInvite.Email &&
                            x.Created == false, x => x);
                    if (invitedUser != null)
                    {
                        var hoursPassed = (DateTime.Now - invitedUser.CreatedAt).TotalHours;
                        if (hoursPassed <= 24)
                        {
                            response.Success = false;
                            response.Errors.Add("User already invited in the last 24 hours");
                            return new Response(response);
                        }
                    }


                    // Get the HTML template that will be sent as email
                    #region Get invite user HTML template 
                    string baseDirectory = Directory.GetCurrentDirectory();
                    string inviteUserTemplatePath = baseDirectory;
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    _logger.LogInformation($"InviteUser -> Creating path file for {env} ENVIRONMENT");
                    if (env == "Development")
                    {
                        inviteUserTemplatePath = Path.Combine(baseDirectory, "..\\Common\\EmailTemplates", "invite_user_template.html");
                    }
                    else
                    {
                        inviteUserTemplatePath = Path.Combine(baseDirectory, "EmailTemplates", "invite_user_template.html");
                    }


                    string htmlContent = "";

                    if (File.Exists(inviteUserTemplatePath))
                    {
                        _logger.LogInformation($"InviteUser -> Reading the file content from {inviteUserTemplatePath}");

                        using (StreamReader reader = new StreamReader(inviteUserTemplatePath))
                        {
                            htmlContent = await reader.ReadToEndAsync();
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"InviteUser -> Couldn't find the file at {inviteUserTemplatePath}");
                        throw new Exception($"File not found at {inviteUserTemplatePath}");
                    }
                    #endregion

                    // get the current user
                    var currentLoggedInUserId = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;

                    if (string.IsNullOrEmpty(currentLoggedInUserId))
                    {
                        _logger.LogInformation("InviteUser -> Can't get the current logged in user");

                        response.Success = false;
                        response.Errors.Add("User do not exist");
                        return new Response(response);
                    }

                    // get the token so we can send it as a query param in ACTION_URL. We will need it later to verify the next request
                    string token = "";
                    string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                    if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ")) {
                        token = authorizationHeader.Substring("Bearer ".Length).Trim();
                    }

                    if (string.IsNullOrEmpty(token))
                    {
                        _logger.LogInformation("InviteUser -> Couldn't get the token from the logged in user ( Couldn't get the jwt token from the request )");
                        response.Success = false;
                        return new Response(response);
                        //throw new Exception("Could not get retrieve the token");
                    }


                    var user = await _userRepository.GetAllQueryable()
                                .Include(x => x.Company)
                            .Where(x => x.Id == Guid.Parse(currentLoggedInUserId))
                            .FirstOrDefaultAsync();
                    var roleName = await _rolesRepository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.userToInvite.RoleId), x => x.Name);

                    var buildActionUrl = $"{Constants.USER_INVITATION_URL}?email={request.userToInvite.Email}&token={token}";


                    //htmlContent = htmlContent.Replace("{{SENDER_EMAIL}}", user.Email);
                    htmlContent = htmlContent.Replace("{{COMPANY_NAME}}", user.Company.Name);
                    htmlContent = htmlContent.Replace("{{USER_ROLE}}", roleName);
                    htmlContent = htmlContent.Replace("{{INVITEE_EMAIL}}", request.userToInvite.Email);
                    htmlContent = htmlContent.Replace("{{ACTION_URL}}", buildActionUrl);


                    // send email with template
                    #region Send email
                    var mailData = new MailData
                    {
                        EmailToId = request.userToInvite.Email,
                        EmailToName = request.userToInvite.Email,
                        EmailSubject = $"Invitation to {user.Company.Name}",
                        EmailBody = htmlContent,
                    };
                    bool emailWasSent = _mailService.SendMail(mailData);

                    if (!emailWasSent)
                    {
                        _logger.LogInformation("InviteUser -> Something went wrong trying to send the email to: ", mailData.EmailToId);
                        
                        response.Success = false;
                        response.Errors.Add("Couldn't send the email to " + request.userToInvite.Email);
                        return new Response(response);
                    }
                    #endregion

                    // add the information about the invited user to the db IF IT DOESN'T EXIST ALREADY
                    if (invitedUser == null)
                    {
                        await _userInvitationsRepository.AddAsync(new UserInvitations
                        {
                            InviterId = Guid.Parse(currentLoggedInUserId),
                            Email = request.userToInvite.Email,
                            RoleId = Guid.Parse(request.userToInvite.RoleId),
                            Created = false
                        });
                    }
                    else
                    {
                        invitedUser.CreatedAt = DateTime.Now;
                        _userInvitationsRepository.Update(invitedUser);
                    }
                    await _userInvitationsRepository.SaveChangesAsync();

                    _logger.LogInformation("InviteUser -> Finished execution for InviteUser(). Email successfully sent");
                    return new Response(response);
                }, _logger);

            }
        }

        // Response
        public record Response(ApiResponseModel<bool> response);

    }
}
