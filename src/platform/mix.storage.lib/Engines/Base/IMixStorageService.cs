using Microsoft.AspNetCore.Http;

namespace Mix.Storage.Lib.Engines.Base
{
    public interface IMixUploader
    {
        Task<string?> UploadFile(IFormFile file, string? themeName, string? createdBy, CancellationToken cancellationToken = default);
        Task<string?> UploadFileStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default);
    }
}