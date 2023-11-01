using Azure.Storage;
using Azure.Storage.Blobs;
using Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Services.FileService
{
    public class FileService
    {
        // TODO: Move these to Azure Secrets
        private readonly string _storageAccount = "storenexusstorage";
        private readonly string _accessKey = "YHYaf5mOkTnz9I2HpJzfKQE3zWcH5S9vfz60nEiPSvPbkfW0/laa6RxQWNZ2Veza9SY42UhRzxO5+AStA46AjA==";
        private readonly BlobContainerClient _filesContainer;

        public FileService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("store-documents");

        }

        public async Task<List<BlobDto>> ListAsync()
        {
            List<BlobDto> files = new List<BlobDto>();

            await foreach (var file in _filesContainer.GetBlobsAsync())
            {
                string uri = _filesContainer.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType,
                });
            }

            return files;
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
        {
            BlobResponseDto response = new();
            BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            response.Status = $"File {blob.FileName} uploaded successfully";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }

        public async Task<BlobDto?> DownloadAsync(string blobFileName)
        {
            BlobClient file = _filesContainer.GetBlobClient(blobFileName);

            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream streamData = data;

                byte[] blobContent;
                using (var stream = new MemoryStream())
                {
                    streamData.CopyTo(stream);
                    blobContent = stream.ToArray();
                }

                var content = await file.DownloadContentAsync();

                string name = blobFileName;
                string contentType = content.Value.Details.ContentType;

                return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
            }

            return null;
        }

        public async Task<BlobResponseDto> DeleteAsync(string blobFileName)
        {
            var file = _filesContainer.GetBlobClient(blobFileName);

            try
            {
                await file.DeleteAsync();
            }
            catch (Exception e)
            {
                // TODO: log the exception
                return new BlobResponseDto { Error = true, Status = $"File: {blobFileName} does not exist" };
            }

            return new BlobResponseDto { Error = false, Status = $"File: {blobFileName} has been successfully deleted" };
        }
    }
}
