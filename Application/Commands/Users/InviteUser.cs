﻿using Infrastructure.Repositories.Contracts;
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

            private readonly ILogger<Handler> _logger;
            private readonly IMailService _mailService;
            private readonly IUserService _userService;


            public Handler(
                ILogger<Handler> logger,
                IHttpContextAccessor httpContextAccessor,
                IUserRepository userRepository,
                IRolesRepository rolesRepository,
                IUserDetailsRepository userDetailsRepository,
                IMailService mailService,
                IUserService userService
                )
            {
                _logger = logger;
                _httpContextAccessor = httpContextAccessor;
                _userRepository = userRepository;
                _rolesRepository = rolesRepository;
                _userDetailsRepository = userDetailsRepository;
                _mailService = mailService;
                _userService = userService;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                return await ExecuteFunc.TryExecute<Task<Response>>(async () =>
                {
                    _logger.LogInformation("Started execution for InviteUser started");
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
                    //string executableDirectory = Path.Combine(AppContext.BaseDirectory, "invite_user_template.html");
                    //_logger.LogInformation($"InviteUser -> Getting root path for executableDirectory: {executableDirectory}");

                    //string solutionRoot = Path.GetFullPath(Path.Combine(executableDirectory, @"..\..\..\..\.."));
                    //_logger.LogInformation($"InviteUser -> Getting root path for solutionRoot: {solutionRoot}");

                    //string htmlFilePath = Path.Combine(solutionRoot, "Common", "EmailTemplates", "invite_user_template.html");
                    //string htmlContent;
                    //using (StreamReader reader = new StreamReader(htmlFilePath))
                    //{
                    //    htmlContent = await reader.ReadToEndAsync();
                    //}

                    string baseDirectory = Directory.GetCurrentDirectory();
                    string inviteUserTemplatePath = Path.Combine(baseDirectory, "Common", "EmailTemplates", "invite_user_template.html");

                    _logger.LogInformation($"InviteUser -> Test -> Getting baseDirectory path: {baseDirectory}");
                    _logger.LogInformation($"InviteUser -> Test 2 -> inviteUserTemplate = {inviteUserTemplatePath}");

                    var directory = Directory.GetCurrentDirectory();
                    string a = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
                    string htmlPath = Path.Combine(a, "Common", "EmailTemplates", "invite_user_template.html");


                    var files = Directory.EnumerateFiles(baseDirectory);
                    var folders = Directory.EnumerateDirectories(baseDirectory);

                    _logger.LogInformation($"InviteUser ---> The following files from {baseDirectory} are:");

                    foreach( var file in files)
                    {
                        _logger.LogInformation($"file = {file}");
                    }

                    _logger.LogInformation($"InviteUser ---> And now ... Let's see the folders / directories for {baseDirectory}:");

                    foreach (var folder in folders)
                    {
                        _logger.LogInformation($"file = {folder}");
                    }






                    string htmlContent;

                    if (File.Exists(htmlPath))
                    {
                        _logger.LogInformation($"InviteUser -> The file indeed --- EXIST and it looks like this: ");
                    }
                    else
                    {
                        _logger.LogInformation("______________ Does not exist _______________Sorry");
                    }
                    using (StreamReader reader = new StreamReader(inviteUserTemplatePath))
                    {
                        htmlContent = await reader.ReadToEndAsync();
                        _logger.LogInformation($"hc = ${htmlContent}");
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
                        _logger.LogInformation("Something went wrong trying to send the email to: ", mailData.EmailToId);
                        
                        response.Success = false;
                        response.Errors.Add("Couldn't send the email to " + request.userToInvite.Email);
                        return new Response(response);
                    }

                    _logger.LogInformation("Finished execution for InviteUser(). Email successfully sent");
                    return new Response(response);
                }, _logger);


                
            }
        }

        // Response
        public record Response(ApiResponseModel<bool> response);

    }
}
