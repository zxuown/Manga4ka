using AutoMapper;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
namespace Manga4ka.Business.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorDto, Author>();
        CreateMap<Genre, GenreDto>();
        CreateMap<GenreDto, Genre>();
        CreateMap<Manga, MangaDto>();
        CreateMap<MangaDto, Manga>();
        CreateMap<RegisterDto, User>();
    }
}
