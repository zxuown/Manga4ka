namespace Manga4ka.Data.Interfaces;

public interface IUnitOfWork
{
    IAuthorRepository Authors { get; }

    IMangaRepository Manga { get; }

    IGenreRepository Genres { get; }

    IUserRepository Users { get; }

    ICommentRepository Comments { get; }

    IRatingRepository Rating { get; }
    Task SaveAsync();
}
