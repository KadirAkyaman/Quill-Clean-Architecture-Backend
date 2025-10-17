using Microsoft.AspNetCore.Http;

namespace Quill.Application.Interfaces.Infrastructure
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Uploads a file to the storage and returns the public URL.
        /// </summary>
        /// <param name="file">The file to upload, represented as IFormFile.</param>
        /// <param name="subfolder">The subfolder within the storage (e.g., "profiles", "posts").</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The publicly accessible URL of the uploaded file.</returns>
        Task<string> UploadFileAsync(IFormFile file, string subfolder, CancellationToken cancellationToken);
    }
}