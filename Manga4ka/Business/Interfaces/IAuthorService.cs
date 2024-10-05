using Manga4ka.Business.Models;

namespace Manga4ka.Business.Interfaces;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDto>> SearchAuthorsAsync(string query);
    Task AddAuthorAsync(AuthorDto entity);
    Task UpdateAuthorAsync(AuthorDto entity);
    Task DeleteAuthorAsync(int id);
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
    Task<AuthorDto> GetAuthorByIdAsync(int id);
}
