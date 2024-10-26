using BCinema.Domain.Interfaces.IServices;
using Google.Cloud.Storage.V1;

namespace BCinema.Infrastructure.Services
{
    public class FirebaseStorageService : IFileStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName = "bcinema-39c05.appspot.com";

        public FirebaseStorageService(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            var storageObject = await _storageClient.UploadObjectAsync(
                _bucketName, fileName, "image/jpeg", imageStream);

            return storageObject.MediaLink;
        }
    }
}
