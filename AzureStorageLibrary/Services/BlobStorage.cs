using Azure.Storage.Blobs;
using AzureStorageLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    public class BlobStorage : IBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorage()
        {
            _blobServiceClient = new BlobServiceClient(ConnectionStrings.AzureStorageConnectionString);
        }

        public string BlobUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task DeleteAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<Stream> DownloadAsync(string fileName, EContainerName eContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());

            var blobClient = containerClient.GetBlobClient(fileName);

            var info = await blobClient.DownloadToAsync(fileName);

            return info.ContentStream;


        }

        public Task<List<string>> GetLogAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetNames(EContainerName eContainerName)
        {
            throw new NotImplementedException();
        }

        public Task SetLog(string text, string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task UploadAsync(Stream fileStream, string fileName, EContainerName eContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());

            await containerClient.CreateIfNotExistsAsync();

            await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream);
        }
    }
}
