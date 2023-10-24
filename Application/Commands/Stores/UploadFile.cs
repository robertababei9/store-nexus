using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;
using Microsoft.AspNetCore.Http;
using Domain.Dto.Stores;
using Common.Models;
using Application.Services.FileService;

namespace Application.Commands.Users
{
    public static class UploadFile
    {
        // Command
        public record Command(UploadFileDto uploadFileDto) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Command, Response>
        {
            protected FileService _fileService { get; set; }
            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            protected IStoreDocumentsRepository _storeDocumentsRepository { get; set; }
            protected IUserRepository _userRepository { get; set; }

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                FileService fileService, 
                IStoreDocumentsRepository storeDocumentsRepository,
                IUserRepository userRepository)
            {
                _fileService = fileService;
                _httpContextAccessor = httpContextAccessor;
                _storeDocumentsRepository = storeDocumentsRepository;
                _userRepository = userRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<StoreDocumentDto>();

                var file = request.uploadFileDto.Blob;

                // let's upload the file to cloud
                // TODO: try ... catch
                var uploadResponse = await _fileService.UploadAsync(file);
                
                // get the user who uploaded it
                var currentLoggedInUserId = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
                if (currentLoggedInUserId != null)
                {
                    var userFullName = await _userRepository
                        .FirstOrDefaultAsync(x => x.Id == Guid.Parse(currentLoggedInUserId), x => x.Name);

                    var storeDocumentsEntity = new StoreDocuments
                    {
                        Name = uploadResponse.Blob.Name,
                        Uri = uploadResponse.Blob.Uri,
                        StoreId = request.uploadFileDto.StoreId,
                        UploadedBy = userFullName
                    };

                    await _storeDocumentsRepository.AddAsync(storeDocumentsEntity);
                    await _storeDocumentsRepository.SaveChangesAsync();

                    response.Data = new StoreDocumentDto
                    {
                        Name = storeDocumentsEntity.Name,
                        UploadedAt = storeDocumentsEntity.CreatedAt.ToString("dd-MMM-yyyy"),
                        UploadedBy = storeDocumentsEntity.UploadedBy,
                        Uri = storeDocumentsEntity.Uri
                    };

                    return new Response(response);
                }
                else
                {
                    response.Success = false;
                    response.Errors.Add("Could not find the user who uploaded the file");
                    return new Response(response);
                }

                response.Success = false;
                response.Errors.Add("Something went wrong trying to upload the file");
                return new Response(response);


            }
        }

        // Response
        public record Response(ApiResponseModel<StoreDocumentDto> response);
    }
}
