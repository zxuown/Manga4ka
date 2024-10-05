using Manga4ka.Data;
using Manga4ka.Data.Repositories;
using Manga4ka.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Manga4ka.Business.Mapper;
using Manga4ka.Business.Services;
using Manga4ka.Business.Interfaces;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod());
});
builder.Services.AddDbContext<Manga4kaContext>(options =>
{
    options.UseSqlite("Data Source=manga4ka.db");
    SQLitePCL.Batteries.Init();
});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 209715200;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 209715200;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Manga4ka API", Version = "v1" });
});

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<IUserRepository,  UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IMangaService, MangaService>();
builder.Services.AddScoped<IPdfFileService, PdfService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var key = Encoding.ASCII.GetBytes(builder.Configuration["Tokens:Token"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manga4ka API V1");
    });
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "PdfFiles")),
    RequestPath = "/PdfFiles"
});
app.MapGet("/", async context =>
{
    context.Response.Redirect("/swagger/index.html");
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowMyOrigin");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
