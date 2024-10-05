namespace Manga4ka.Data.Interfaces;

public interface IUnitOfWork
{
    IAuthorRepository Authors { get; }

    IMangaRepository Manga { get; }

    IGenreRepository Genres { get; }

    IUserRepository Users { get; }
    Task SaveAsync();
}
