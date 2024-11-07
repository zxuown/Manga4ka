using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manga4ka.Api.Controllers;

[ApiController]
[Route("/pdffile")]
public class PdfFileController(IPdfFileService pdfFileService) : ControllerBase
{
    private readonly IPdfFileService _pdfFileService = pdfFileService;

    [Authorize(Roles = "Admin")]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] FileWrapperDto file)
    {
        var url = await _pdfFileService.UploadAsync(file.file);

        return Ok(new
        {
            Url = url,
        });
    }

    [HttpGet("file")]
    public async Task<IActionResult> GetPdfFile([FromQuery] int id)
    {
        try
        {
            var pdfFile = await _pdfFileService.GetPdfFileAsync(id);
            return pdfFile;
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{mangaId}")]
    public async Task<IActionResult> DeleteFile(int mangaId)
    {
        await _pdfFileService.Remove(mangaId);
        return Ok();
    }
}
