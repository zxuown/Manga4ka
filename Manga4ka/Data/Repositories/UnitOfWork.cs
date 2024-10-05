namespace Manga4ka.Data.Repositories;
using Data.Interfaces;
public class UnitOfWork(IAuthorRepository authorRepository, IGenreRepository genreRepository, IMangaRepository mangaRepository, 
   IUserRepository userRepository, Manga4kaContext context) : IUnitOfWork
{
    public readonly Manga4kaContext _context = context;
    public IAuthorRepository Authors { get; } = authorRepository;

    public IGenreRepository Genres { get; } = genreRepository;

    public IMangaRepository Manga { get; } = mangaRepository;

    public IUserRepository Users { get; } = userRepository;

    public async Task SaveAsync()
    {
       await _context.SaveChangesAsync();
    }
}
