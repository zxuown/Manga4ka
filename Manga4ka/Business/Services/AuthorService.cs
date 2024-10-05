using AutoMapper;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using Manga4ka.Data.Repositories;

namespace Manga4ka.Business.Services;

public class AuthorService(IUnitOfWork unitOfWork, IMapper mapper) : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly IMapper _mapper = mapper;
    
    public async Task<IEnumerable<AuthorDto>> SearchAuthorsAsync(string query)
    {
        var filteredAuthors = await _unitOfWork.Authors.Search(query);
        return _mapper.Map<IEnumerable<AuthorDto>>(filteredAuthors);
    }
    public async Task AddAuthorAsync(AuthorDto author)
    {
        await _unitOfWork.Authors.AddAsync(_mapper.Map<Author>(author));
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAuthorAsync(AuthorDto author)
    {
        await _unitOfWork.Authors.UpdateAsync(_mapper.Map<Author>(author));
        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteAuthorAsync(int id)
    {
        await _unitOfWork.Authors.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<AuthorDto> GetAuthorByIdAsync(int id)
    {
        Author author = await _unitOfWork.Authors.GetByIdAsync(id);
        return _mapper.Map<AuthorDto>(author);
    }
    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        IEnumerable<Author> authors = await _unitOfWork.Authors.GetAllAsync();
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }
}
