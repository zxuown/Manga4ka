namespace Manga4ka.Data.Repositories;
using Data.Interfaces;
using Data.Entities;
public class AuthorRepository(Manga4kaContext context) : BaseRepository<Author>(context), IAuthorRepository
{
    public async Task<IEnumerable<Author>> Search(string query)
    {
        var authors = await GetAllAsync();
        return authors.Where(author => author.Name.ToLower().Contains(query.ToLower()) ||author.Lastname.ToLower().Contains(query.ToLower()));
    }

}
