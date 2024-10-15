using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;

namespace Manga4ka.Business.Interfaces;

public interface ICommentService
{
    Task AddCommentAsync(CommentDto entity);
    Task UpdateCommentAsync(CommentDto entity);
    Task DeleteCommentAsync(int id);
    Task<IEnumerable<CommentDto>> GetAllCommentsByMangaIdAsync(int mangaId);
    Task<CommentDto> GetCommentByIdAsync(int id);
}
