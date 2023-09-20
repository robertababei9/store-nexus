using Infrastructure.Repositories.Contracts;
using MediatR;
using Domain.Dto.CompanyDto;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Commands.Company
{
    public static class CreateCompany
    {
        // Command
        public record Command(CreateCompanyDto companyDto) : IRequest<Guid>;

        // Handler
        public class Handler : IRequestHandler<Command, Guid>
        {
            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            protected ICompanyRepository _companyRepository { get; set; }
            protected IUserRepository _userRepository { get; set; }

            public Handler(IHttpContextAccessor httpContextAccessor, ICompanyRepository companyRepository, IUserRepository userRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _companyRepository = companyRepository;
                _userRepository = userRepository;
            }


            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _companyRepository.AddAsync(new Domain.Entities.Company
                {
                    Name = request.companyDto.Name,
                    NoEmployees = request.companyDto.NoEmployees,
                    Type = request.companyDto.Type,
                    Address = request.companyDto.Address,
                    Contact = request.companyDto.Contact,
                    WebsiteUrl = request.companyDto.WebsiteUrl
                });

                var currentLoggedInUserId = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
                if (string.IsNullOrEmpty(currentLoggedInUserId))
                {
                    return Guid.Empty;
                }

                var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(currentLoggedInUserId), x => x);
                user.CompanyId = company.Id;
                _userRepository.Update(user);

                await _companyRepository.SaveChangesAsync();


                return company.Id;
            }
        }

    }
}
