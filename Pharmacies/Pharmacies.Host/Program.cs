using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pharmacies.Application;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Dto.Reference;
using Pharmacies.Application.Interfaces;
using Pharmacies.Application.Services;
using Pharmacies.Application.Services.Reference;
using Pharmacies.EntityFramework.MySqlConfiguration;
using Pharmacies.EntityFramework.Repositories.Mocks;
using Pharmacies.EntityFramework.Repositories.Mocks.Reference;
using Pharmacies.EntityFramework.Repositories.MySql;
using Pharmacies.EntityFramework.Repositories.MySql.Reference;
using Pharmacies.Interfaces;
using Pharmacies.Model;
using Pharmacies.Model.Reference;
using PharmaceuticalGroupReferenceRepository = Pharmacies.EntityFramework.Repositories.MySql.Reference.PharmaceuticalGroupReferenceRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database context
builder.Services.AddDbContext<PharmacyMySqlContext>(options =>
{
    options
        .UseMySql(
            builder.Configuration.GetConnectionString("mysql"),
            new MySqlServerVersion(new Version(9, 0, 1)
            ));
});

// Mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Repositories
builder.Services.AddTransient<IRepository<Pharmacy, int>, PharmacyRepository>();
builder.Services.AddTransient<IRepository<Position, int>, PositionRepository>();
builder.Services.AddTransient<IRepository<Price, int>, PriceRepository>();
builder.Services.AddTransient<IRepository<PharmaceuticalGroup, int>, PharmaceuticalGroupRepository>();
builder.Services.AddTransient<IRepository<ProductGroup, int>, ProductGroupRepository>();
builder.Services.AddTransient<IReferenceRepository<Position, PharmaceuticalGroup,int ,int>, PharmaceuticalGroupReferenceRepository>();

// Services
builder.Services.AddTransient<IReferenceService<int ,int>, PharmaceuticalGroupReferenceService>();
builder.Services.AddTransient<IEntityService<PharmaceuticalGroupDto, int>, PharmaceuticalGroupService>();
builder.Services.AddTransient<IEntityService<PharmacyDto, int>, PharmacyService>();
builder.Services.AddTransient<IEntityService<PositionDto, int>, PositionService>();
builder.Services.AddTransient<IEntityService<PriceDto, int>, PriceService>();
builder.Services.AddTransient<IEntityService<ProductGroupDto, int>, ProductGroupService>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => { policy.AllowAnyOrigin(); policy.AllowAnyMethod(); policy.AllowAnyHeader(); }));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pharmacies API", Version = "v1" });
        
    // Включаем XML комментарии (если используются)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacies API v1");
    });
}

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
