using EduOnline.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace EduOnline.Services
{
    public class AzureBlobHelper : IAzureBlobHelper
    {
        #region Dependencies & Properties
        private readonly CloudBlobClient _cloudBlobClient; 

        public AzureBlobHelper(IConfiguration configuration)
        {
            string keys = configuration["Blob:AzureStorage"];
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(keys);
            _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        #endregion

        #region Methods
        public async Task<Guid> UploadAzureBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadAzureBlobAsync(stream, containerName);

        }

        public async Task<Guid> UploadAzureBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
            return await UploadAzureBlobAsync(stream, containerName);
        }

        private async Task<Guid> UploadAzureBlobAsync(Stream stream, string containerName)
        {
            Guid name = Guid.NewGuid();

            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");
            await blockBlob.UploadFromStreamAsync(stream);
            return name;
        }

        public async Task DeleteAzureBlobAsync(Guid id, string containerName)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{id}");
            await blockBlob.DeleteAsync();
        }

        #endregion
    }
}
