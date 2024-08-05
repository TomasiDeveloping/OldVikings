using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Interfaces;
using OldVikings.Api.Profiles;
using OldVikings.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OldVikingsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OldVikings"));
});

builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<GreetingProfile>();
});

builder.Services.AddScoped<IGreetingRepository, GreetingRepository>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
