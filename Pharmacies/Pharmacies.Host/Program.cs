using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;
using Pharmacies.Repositories.Mocks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the repository as a singleton or scoped service
builder.Services.AddSingleton<IRepository<Pharmacy, int>, PharmacyRepositoryMock>();
builder.Services.AddSingleton<IRepository<Position, int>, PositionRepositoryMock>();
builder.Services.AddSingleton<IRepository<Price, int>, PriceRepositoryMock>();
builder.Services.AddSingleton<IRepository<PharmaceuticalGroup, int>, PharmaceuticalGroupRepositoryMock>();
builder.Services.AddSingleton<IRepository<ProductGroup, int>, ProductGroupRepositoryMock>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacies API V1");
    });
}

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
