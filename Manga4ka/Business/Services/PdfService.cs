using Manga4ka.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Business.Services;

public class PdfService(IWebHostEnvironment webHostEnvironment) : IPdfFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    public async Task<string> UploadAsync(IFormFile pdfFile)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);
        string path = Path.Combine("PdfFiles", fileName);

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
    public async Task<FileResult> GetPdfFileAsync(string pdfFilePath)
    {
        var fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, pdfFilePath);

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
    public void Remove(string path)
    {
        File.Delete(path);
    }
}
