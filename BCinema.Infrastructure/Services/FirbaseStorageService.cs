using BCinema.Domain.Interfaces.IServices;
using Google.Cloud.Storage.V1;

namespace BCinema.Infrastructure.Services
{
    public class FirebaseStorageService(StorageClient storageClient) : IFileStorageService
    {
        private const string BucketName = "bcinema-39c05.appspot.com";

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            await storageClient.UploadObjectAsync(
                BucketName, fileName, "image/jpeg", imageStream);

            return $"https://firebasestorage.googleapis.com/v0/b/{BucketName}/o/{fileName}?alt=media";
        }
        
        public async Task DeleteImageAsync(string fileName)
        {
            await storageClient.DeleteObjectAsync(BucketName, fileName);
        }
        
        public async Task<string> UpdateImageAsync(Stream imageStream, string fileName, string existingFileName)
        {
            await DeleteImageAsync(existingFileName);
            return await UploadImageAsync(imageStream, fileName);
        }
    }
}
