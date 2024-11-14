namespace BCinema.Domain.Interfaces.IServices
{
    public interface IFileStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName);
        Task DeleteImageAsync(string fileName);
        Task<string> UpdateImageAsync(Stream imageStream, string fileName, string existingFileName);
    }
}
