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
    public static class DeleteFile
    {
        // Query
        public record Query(string blobFileName) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IStoreDocumentsRepository _storeDocumentsRepository { get; set; }
            protected FileService _fileService { get; set; }
            public Handler(IStoreDocumentsRepository storeDocumentsRepository, FileService fileService)
            {
                _storeDocumentsRepository = storeDocumentsRepository;
                _fileService = fileService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<bool>();

                var fileInDb = await _storeDocumentsRepository.FirstOrDefaultAsync(x => x.Name == request.blobFileName, x => x);

                if (fileInDb == null)
                {
                    response.Success = false;
                    response.Errors.Add("File does not exist");
                    
                    return new Response(response);
                }

                var deletedFileStatus = await _fileService.DeleteAsync(request.blobFileName);

                if (deletedFileStatus.Error)
                {
                    response.Success = false;
                    response.Errors.Add("File couldn't be deleted from the cloud");

                    return new Response(response);
                }


                _storeDocumentsRepository.Delete(fileInDb);
                await _storeDocumentsRepository.SaveChangesAsync();

                return new Response(response);
            }
        }

        // Response
        public record Response(ApiResponseModel<bool> response);
    }
}
