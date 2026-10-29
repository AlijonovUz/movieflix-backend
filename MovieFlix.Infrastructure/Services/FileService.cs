using Microsoft.AspNetCore.Http;
using MovieFlix.Application.Common.Interfaces;

namespace MovieFlix.Infrastructure.Services;

public class FileService : IFileService
{
    public async Task<string?> SaveImageAsync(
        IFormFile? imageFile, 
        string folderName,
        string? oldImageUrl = null,
        CancellationToken cancellationToken = default)
    {
        if (imageFile is null || imageFile.Length == 0)
        {
            return oldImageUrl;
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var extension = Path.GetExtension(imageFile.FileName);
        var uniqueName = $"{Guid.NewGuid().ToString()}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(oldImageUrl))
        {
            DeleteImageFile(oldImageUrl);
        }

        return $"/uploads/{folderName}/{uniqueName}";
    }

    public void DeleteImageFile(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return;
        }

        var relativePath = imageUrl.TrimStart('/');
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}