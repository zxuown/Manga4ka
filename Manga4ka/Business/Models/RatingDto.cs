﻿using Manga4ka.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga4ka.Business.Models;

public record RatingDto
{
    public int Id { get; init; }
    public UserDto User { get; init; }
    public MangaDto Manga { get; init; }
    public int Value { get; init; }
}
