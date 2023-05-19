using WarhammerTournaments.Interfaces;
using Imagekit.Sdk;

namespace WarhammerTournaments.Services;

public class ImageUploadService : IImageUploadService
{
    private readonly ImagekitClient _imagekitClient;
    public static string DefaultImageUrl { get; }

    static ImageUploadService()
    {
        DefaultImageUrl = "https://ik.imagekit.io/pankrat/DefaultImage.jpg?updatedAt=1684483827019";
    }

    public ImageUploadService()
    {
        var publicKey = System.Environment.GetEnvironmentVariable("ImagekitPublicKey");
        var privateKey = System.Environment.GetEnvironmentVariable("ImagekitPrivateKey");
        var urlEndPoint = System.Environment.GetEnvironmentVariable("ImagekitUrlEndPoint");
        _imagekitClient = new ImagekitClient(publicKey, privateKey, urlEndPoint);
    }

    public async Task<Result> UploadAsync(IFormFile? image)
    {
        var uploadResult = new Result();

        if (image.Length > 0)
        {
            await using var stream = image.OpenReadStream();
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var fileName = Path.GetFileNameWithoutExtension(image.FileName);
            var extension = Path.GetExtension(image.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

            var newImage = new FileCreateRequest
            {
                file = memoryStream.ToArray(),
                fileName = fileName,
            };

            uploadResult = await _imagekitClient.UploadAsync(newImage);
        }

        return uploadResult;
    }

    public async Task<ResultDelete?> DeleteAsync(string fileId, string url)
    {
        if (url != DefaultImageUrl)
        {
            if (fileId == null || fileId == String.Empty)
            {
                return null;
            }

            return await _imagekitClient.DeleteFileAsync(fileId);
        }

        return null;
    }

    public Result Find(string url)
    {
        return _imagekitClient.GetFileDetail(url);
    }
}