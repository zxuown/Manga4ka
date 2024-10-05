using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Business.Services;
using Manga4ka.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Api.Controllers;

[ApiController]
[Route("/pdffile")]
public class PdfFileController(IPdfFileService pdfFileService, IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IPdfFileService _pdfFileService = pdfFileService;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpPost("upload")]
    public async Task<ActionResult> UploadFile([FromForm] FileWrapperDto file)
    {
        var url = await _pdfFileService.UploadAsync(file.file);

        return Ok(new
        {
            Url = url,
        });
    }

    [HttpGet("file")]
    public async Task<ActionResult> GetPdfFile([FromQuery] int id)
    {
        try
        {
            var manga = await _unitOfWork.Manga.GetByIdAsync(id);
            var pdfFile = await _pdfFileService.GetPdfFileAsync(manga.Pdfile);
            return pdfFile;
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("delete/{mangaId}")]
    public async Task DeleteFile(int mangaId)
    {
        var manga = await _unitOfWork.Manga.GetByIdAsync(mangaId);
        _pdfFileService.Remove(manga.Pdfile);
    }
}
