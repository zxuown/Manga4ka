using AutoMapper;
using Manga4ka.Business.Interfaces;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;

namespace Manga4ka.Business.Services;

public class CommentService(IUnitOfWork unitOfWork, IMapper mapper) : ICommentService
{
    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMapper _mapper { get; } = mapper;
    public async Task AddCommentAsync(CommentDto commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);
        comment.User = await _unitOfWork.Users.GetByIdAsync(commentDto.User.Id);
        comment.Manga = await _unitOfWork.Manga.GetByIdAsync(commentDto.Manga.Id);
        comment.DatePublished = TimeZoneInfo.ConvertTimeFromUtc(commentDto.DatePublished, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        await _unitOfWork.Comments.AddAsync(comment);
        await _unitOfWork.SaveAsync();
    }
    public async Task UpdateCommentAsync(CommentDto entity)
    {
        await _unitOfWork.Comments.UpdateAsync(_mapper.Map<Comment>(entity));
        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteCommentAsync(int id)
    {
        await _unitOfWork.Comments.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }
    public async Task<IEnumerable<CommentDto>> GetAllCommentsByMangaIdAsync(int mangaId)
    {
        return _mapper.Map<IEnumerable<CommentDto>>(await _unitOfWork.Comments.GetAllCommentsByMangaIdAsync(mangaId));
    }
    public async Task<CommentDto> GetCommentByIdAsync(int id)
    {
        return _mapper.Map<CommentDto>(await _unitOfWork.Comments.GetByIdAsync(id));
    }
}
