namespace WarhammerTournaments.Interfaces;

public interface IImageUploadService
{
    public Task<string> Upload(IFormFile image);
}