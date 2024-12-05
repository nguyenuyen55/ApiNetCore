using Microsoft.EntityFrameworkCore;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.Respository;
using SupperHeroAPI_Dotnet8.Respository.impRepository;
using SupperHeroAPI_Dotnet8.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//config repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
