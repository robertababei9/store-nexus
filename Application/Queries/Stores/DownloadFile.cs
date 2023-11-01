using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Domain.Dto.Stores;
using Application.Services.FileService;

namespace Application.Queries.Stores
{
    public static class DownloadFile
    {
        // Query
        public record Query(string blobFileName) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IStoreRepository _storeRepository { get; set; }
            protected FileService _fileService { get; set; }
            public Handler(IStoreRepository storeRepository, FileService fileService)
            {
                _storeRepository = storeRepository;
                _fileService = fileService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<BlobDto?>();

                var downloadedBlob = await _fileService.DownloadAsync(request.blobFileName);

                if (downloadedBlob == null)
                {
                    response.Success = false;
                }

                response.Data = downloadedBlob;

                return new Response(response);
            }
        }

        // Response
        public record Response(ApiResponseModel<BlobDto?> response);
    }
}
