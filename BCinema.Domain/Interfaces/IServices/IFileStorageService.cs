namespace BCinema.Domain.Interfaces.IServices
{
    public interface IFileStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName);
    }
}
