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
            protected IUserDetailsRepository _userDetailsRepository { get; set; }

            private readonly IMailService _mailService;
            private readonly IUserService _userService;


            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUserRepository userRepository,
                IRolesRepository rolesRepository,
                IUserDetailsRepository userDetailsRepository,
                IMailService mailService,
                IUserService userService)
            {
                _httpContextAccessor = httpContextAccessor;
                _userRepository = userRepository;
                _rolesRepository = rolesRepository;
                _userDetailsRepository = userDetailsRepository;
                _mailService = mailService;
                _userService = userService;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<bool>();

                // check if another email already exist
                bool emailAlreadyExist = (await _userRepository.FirstOrDefaultAsync(x => x.Email == request.userToInvite.Email, x => x)) != null;
                if (emailAlreadyExist)
                {
                    response.Success = false;
                    response.Errors.Add("Email address is already in use");
                    return new Response(response);
                }

                // read the content of email template
                string executableDirectory = Path.Combine(AppContext.BaseDirectory, "invite_user_template.html");
                string solutionRoot = Path.GetFullPath(Path.Combine(executableDirectory, @"..\..\..\..\.."));
                string htmlFilePath = Path.Combine(solutionRoot, "Common", "EmailTemplates", "invite_user_template.html");

                string htmlContent;
                using (StreamReader reader = new StreamReader(htmlFilePath))
                {
                    htmlContent = await reader.ReadToEndAsync();
                }

                // replace content
                var currentLoggedInUserId = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
                if (string.IsNullOrEmpty(currentLoggedInUserId))
                {
                    response.Success = false;
                    response.Errors.Add("User do not exist");
                    return new Response(response);
                }

                var user = await _userRepository.GetAllQueryable()
                            .Include(x => x.Company)
                        .Where(x => x.Id == Guid.Parse(currentLoggedInUserId))
                        .FirstOrDefaultAsync();
                var role = await _rolesRepository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.userToInvite.RoleId), x => x.Name);
                
                htmlContent = htmlContent.Replace("{{SENDER_EMAIL}}", user.Email);
                htmlContent = htmlContent.Replace("{{COMPANY_NAME}}", user.Company.Name);
                htmlContent = htmlContent.Replace("{{USER_ROLE}}", role);
                htmlContent = htmlContent.Replace("{{INVITEE_EMAIL}}", request.userToInvite.Email);
                htmlContent = htmlContent.Replace("{{ACTION_URL}}", "https://store-nexus-app.netlify.app/");


                // send email with template
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
                    response.Success = false;
                    response.Errors.Add("Couldn't send the email to " + request.userToInvite.Email);
                    return new Response(response);
                }

                return new Response(response);
            }
        }

        // Response
        public record Response(ApiResponseModel<bool> response);

    }
}
