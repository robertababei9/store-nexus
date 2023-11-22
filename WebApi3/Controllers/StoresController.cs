using Application.Commands.Stores;
using Application.Commands.Users;
using Application.Queries.Stores;
using Application.Services.FileService;
using Authorization.Attributes;
using Domain.Dto.Stores;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IMediator _mediator;
        private readonly FileService _fileService;

        public StoresController(
            ILogger<InvoicesController> logger,
            IMediator mediator,
            FileService fileService
        )
        {
            _logger = logger;
            _mediator = mediator;
            _fileService = fileService;
        }

        #region GETS
        [PermissionsAuthorize(nameof(RolePermissions.ViewStore))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllStores.Query());

            return Ok(result.response);
        }
        [PermissionsAuthorize(nameof(RolePermissions.ViewStore))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllStoreLocationData()
        {
            var result = await _mediator.Send(new GetAllStoreLocationData.Query());

            return Ok(result.response);
        }

        [PermissionsAuthorize(nameof(RolePermissions.ViewStore))]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetById.Query(id));

            return Ok(response.response);
        }

        [HttpGet("[action]/{storeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllFiles(Guid storeId)
        {
            var result = await _mediator.Send(new GetAllFiles.Query(storeId));

            return Ok(result.response);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetStoreStatuses()
        {
            var result = await _mediator.Send(new GetStoreStatuses.Query());

            return Ok(result.response);
        }

        [HttpGet("[action]/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var result = await _mediator.Send(new DownloadFile.Query(fileName));

            var data = result.response.Data;

            return File(data.Content, data.ContentType, data.Name);
        }

        [HttpGet("[action]/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var result = await _mediator.Send(new DeleteFile.Query(fileName));

            return Ok(result.response);
        }
        #endregion

        #region POSTS
        [PermissionsAuthorize(nameof(RolePermissions.CreateStore))]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreDto storeToCreate)
        {
            var result = await _mediator.Send(new CreateStore.Command(storeToCreate));

            return Ok(result.response);

        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Upload(IFormFile data, Guid id)
        {
            var result = await _mediator.Send(new UploadFile.Command(new UploadFileDto
            {
                Blob = data,
                StoreId = id
            }));
            return Ok(result.response);
        }
        #endregion

        #region PUTS
        [PermissionsAuthorize(nameof(RolePermissions.EditStore))]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Edit([FromBody] CreateStoreDto storeToEdit)
        {
            var result = await _mediator.Send(new EditStore.Command(storeToEdit));

            return Ok(result.response);
        }
        #endregion
    }
}
