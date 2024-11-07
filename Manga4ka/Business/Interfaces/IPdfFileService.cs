using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Business.Interfaces;

public interface IPdfFileService
{
    Task<string> UploadAsync(IFormFile pdfFile);
    
    Task<FileResult> GetPdfFileAsync(int mangaId);

    Task Remove(int mangaId);
}
