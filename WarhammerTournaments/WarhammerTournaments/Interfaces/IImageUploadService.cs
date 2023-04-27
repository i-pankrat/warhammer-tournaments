namespace WarhammerTournaments.Interfaces;

public interface IImageUploadService
{
    public Task<string> Upload(IFormFile? image);
    public void Delete(string image);
}