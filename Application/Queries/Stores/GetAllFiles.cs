
using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Dto.Stores;
using Common.Models;

namespace Application.Queries.Stores
{
    public static class GetAllFiles
    {
        // Query
        public record Query(Guid storeId) : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IStoreDocumentsRepository _storeDocumentsRepository { get; set; }

            public Handler(IStoreDocumentsRepository storeDocumentsRepository)
            {
                _storeDocumentsRepository = storeDocumentsRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<IEnumerable<StoreDocumentDto>>();

                var storeFilesData = _storeDocumentsRepository.GetAllQueryable()
                                .Where(x => x.StoreId == request.storeId)
                                .Select(x => new StoreDocumentDto
                                {
                                    Name = x.Name,
                                    UploadedAt = x.CreatedAt.ToString("dd-MMM-yyyy"),
                                    UploadedBy = x.UploadedBy,
                                    Uri = x.Uri
                                }).ToList();


                response.Data = storeFilesData;

                return new Response(response);
            }
        }


        // Response
        public record Response(ApiResponseModel<IEnumerable<StoreDocumentDto>> response);
    }
}

