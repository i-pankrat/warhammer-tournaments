using WarhammerTournaments.Interfaces;

namespace WarhammerTournaments.Services;

public class ImageUploadService : IImageUploadService
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public ImageUploadService(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public async Task<string> Upload(IFormFile image)
    {
        var wwwRootPath = _hostEnvironment.WebRootPath;
        var fileName = Path.GetFileNameWithoutExtension(image.FileName);
        var extension = Path.GetExtension(image.FileName);
        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        var path = Path.Combine(wwwRootPath, "images");
        path = Path.Combine(path, fileName);

        await using var fileStream = new FileStream(path, FileMode.Create);
        await image.CopyToAsync(fileStream);
        return path;
    }

    public void Delete(string image)
    {
        if (File.Exists(image))
        {
            File.Delete(image);
        }
    }
}