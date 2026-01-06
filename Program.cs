using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Routes;
using salian_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* Init Databse */
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/* For customize error message */
builder.Services.AddLogging();
builder.Services.AddProblemDetails();


// For convert send and recive request as Json format
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        // with this code (case-insensitive)json object in enum
        // options.JsonSerializerOptions.Converters.Add(
        //     new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


// add routes
app.MapUserRoutes("User");
app.MapRoleRoutes("Role");
app.MapLocationRoutes("Location");
app.MapEmployeeRoutes("Employee");

app.Run();
