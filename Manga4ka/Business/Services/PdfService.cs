using Manga4ka.Business.Interfaces;
using Manga4ka.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Business.Services;

public class PdfService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IUnitOfWork unitOfWork) : IPdfFileService
{
    private IWebHostEnvironment _webHostEnvironment { get; } = webHostEnvironment;

    private IConfiguration _configuration { get; } = configuration;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<string> UploadAsync(IFormFile pdfFile)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);
        string path = Path.Combine(_configuration["Files:Pdf"], fileName);

        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            await pdfFile.CopyToAsync(fileStream);
        }

        return path;
    }
    public async Task<FileResult> GetPdfFileAsync(int mangaId)
    {
        var manga = await _unitOfWork.Manga.GetByIdAsync(mangaId);
        var fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, manga.Pdfile);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("File not found.", fullPath);
        }

        var memory = new MemoryStream();
        using (var stream = new FileStream(fullPath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        return new FileStreamResult(memory, "application/pdf")
        {
            FileDownloadName = Path.GetFileName(fullPath)
        };
    }
    public async Task Remove(int mangaId)
    {
        var manga = await _unitOfWork.Manga.GetByIdAsync(mangaId);
        File.Delete(manga.Pdfile);
    }
}
