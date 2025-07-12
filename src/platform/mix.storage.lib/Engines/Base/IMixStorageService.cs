using Microsoft.AspNetCore.Http;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Lib.Engines.Base
{
    public interface IMixUploader
    {
        Task<MixMediaViewModel?> UploadFile(IFormFile file, string? themeName, string? createdBy, CancellationToken cancellationToken = default);
        Task<MixMediaViewModel?> UploadFileStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default);
    }
}