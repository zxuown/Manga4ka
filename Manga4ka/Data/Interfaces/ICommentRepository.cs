using Manga4ka.Data.Entities;

namespace Manga4ka.Data.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
   Task <IEnumerable<Comment>> GetAllCommentsByMangaIdAsync(int mangaId);
}
