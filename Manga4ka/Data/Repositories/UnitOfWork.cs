namespace Manga4ka.Data.Repositories;
using Data.Interfaces;
public class UnitOfWork(IAuthorRepository authorRepository, IGenreRepository genreRepository, IMangaRepository mangaRepository, 
   IUserRepository userRepository, ICommentRepository commentRepository, IRatingRepository ratingRepository, Manga4kaContext context) : IUnitOfWork
{
    private Manga4kaContext _context { get; } = context;
    public IAuthorRepository Authors { get; } = authorRepository;

    public IGenreRepository Genres { get; } = genreRepository;

    public IMangaRepository Manga { get; } = mangaRepository;

    public IUserRepository Users { get; } = userRepository;

    public ICommentRepository Comments { get; } = commentRepository;

    public IRatingRepository Rating { get; } = ratingRepository;
    public async Task SaveAsync()
    {
       await _context.SaveChangesAsync();
    }
}
