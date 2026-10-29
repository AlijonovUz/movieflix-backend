using Microsoft.AspNetCore.Http;

namespace MovieFlix.Application.Common.Interfaces;

public interface IFileService
{
    Task<string?> SaveImageAsync(
        IFormFile formFile,
        string folderName,
        string? oldImageUrl = null,
        CancellationToken cancellationToken = default);
    
    void DeleteImageFile(string? imageUrl);
}