using AutoMapper;
using Manga4ka.Business.Models;
using Manga4ka.Data.Entities;
using Manga4ka.Data.Interfaces;
using Manga4ka.Data.Repositories;
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
        CreateMap<FavoriteMangaDto, FavoriteManga>();
        CreateMap<FavoriteManga, FavoriteMangaDto>();
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<CommentDto, Comment>();
        CreateMap<Comment, CommentDto>();
        CreateMap<RatingDto, Rating>();
        CreateMap<Rating, RatingDto>();
        CreateMap<MangaGenreDto, MangaGenre>();
        CreateMap<MangaGenre, MangaGenreDto>();

    }
}
