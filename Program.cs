using AutoMapper;
using Microsoft.EntityFrameworkCore;
using salian_api.Interface;
using salian_api.Middleware;
using salian_api.Models;
using salian_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* Init Databse */
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/* For customize error message */
builder.Services.AddExceptionHandler<ErrorMessageHandler>();
builder.Services.AddLogging();
builder.Services.AddProblemDetails();


builder.Services.AddScoped<IRoleServices,RoleServices>();
// For convert send and recive request as Json format
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

/* AutoMapper package configuration */
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
