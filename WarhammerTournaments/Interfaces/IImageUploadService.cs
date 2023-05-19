namespace WarhammerTournaments.Interfaces;

public interface IImageUploadService
{
    public Task<Result> UploadAsync(IFormFile? image);
    public Task<ResultDelete> DeleteAsync(string fileId, string url);
    public Result Find(string url);
    public static string DefaultImageUrl { get; }
}