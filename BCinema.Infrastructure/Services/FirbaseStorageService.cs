using System.Net;
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
            var actualFileName = ExtractFileName(fileName);

            try
            {
                var obj = await storageClient.GetObjectAsync(BucketName, actualFileName);

                if (obj != null)
                {
                    await storageClient.DeleteObjectAsync(BucketName, actualFileName);
                }
            }
            catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"File not found: {actualFileName}");
            }
        }
        
        public async Task<string> UpdateImageAsync(Stream imageStream, string fileName, string existingFileName)
        {
            await DeleteImageAsync(existingFileName);
            return await UploadImageAsync(imageStream, fileName);
        }
        
        private static string ExtractFileName(string url)
        {
            if (!url.Contains("/o/")) return url;
            
            var startIndex = url.IndexOf("/o/", StringComparison.Ordinal) + 3;
            var endIndex = url.IndexOf("?alt=media", StringComparison.Ordinal);

            return endIndex == -1 ? url[startIndex..] : url.Substring(startIndex, endIndex - startIndex);
        }
    }
}
