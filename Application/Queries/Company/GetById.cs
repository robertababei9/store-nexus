using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using MediatR;
using Domain.Dto.Company;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Application.ExecutionHelper.Exceptions;
using Application.ExecutionHelper;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Company
{
    public static class GetById
    {
        // Query
        public record Query(Guid companyId) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ILogger<Handler> _logger;
            protected ICompanyRepository _companyRepository { get; set; }
            public Handler(ILogger<Handler> logger,
                ICompanyRepository companyRepository)
            {
                _logger = logger;
                _companyRepository = companyRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return await ExecuteFunc.TryExecute<Task<Response>>(async () =>
                {
                    var response = new ApiResponseModel<CompanyDto>();

                    var companyDto = await _companyRepository
                        .GetAllQueryable()
                            .Include(x => x.Users)
                            .Include(x => x.Stores)
                        .Where(x => x.Id == request.companyId)
                        .Select(x => new CompanyDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            TotalMembers = x.Users.Count(),
                            TotalStores = x.Stores.Count(),
                            ImageUrl = "https://coming-soon.com"
                        })
                        .FirstOrDefaultAsync();

                    if (companyDto == null)
                    {
                        _logger.LogInformation("Could not find a record for company id {request.companyId}");
                        throw new StoreNexusException($"Could not find a record for company id {request.companyId}");
                    }

                    response.Data = companyDto;

                    return new Response(response);
                }, _logger);
            }
        }

        // Response
        public record Response(ApiResponseModel<CompanyDto> response);
    }
}
