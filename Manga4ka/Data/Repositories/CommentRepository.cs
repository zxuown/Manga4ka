using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manga4ka.Data.Repositories;

public class CommentRepository(Manga4kaContext context) : BaseRepository<Comment>(context), ICommentRepository
{
    public async Task<IEnumerable<Comment>> GetAllCommentsByMangaIdAsync(int mangaId)
    {
        return await _entities.Where(x => x.MangaId == mangaId).Include(x => x.User).ToListAsync();
    }
}
