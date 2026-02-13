
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Routes;
using salian_api.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using salian_api.Config.Extentions;
using salian_api.Config;
using Microsoft.AspNetCore.Authorization;
using salian_api.Config.Permissions;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOurSwagger();

/* Init Databse */
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/* For customize error message */
builder.Services.AddLogging();
builder.Services.AddProblemDetails();


// For convert send and recive request as Json format
/*builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
            //new System.Text.Json.Serialization.JsonStringEnumConverter());
    });*/

builder.Services.ConfigureHttpJsonOptions(option =>
{
    option.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IActionTypeService, ActionTypeService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// overwrite permissions
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

//Add CORS
builder.Services.AddCors(
    options => options.AddPolicy("MyLocalhost", builder =>
    builder.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:3000", "http://localhost:5005")));

// Add new configuration
var authConfiguration = builder.Configuration.GetSection("AuthSettings");
builder.Services.Configure<AuthSettings>(authConfiguration);

// scafold Jwt Authorization
var authSettings = authConfiguration.Get<AuthSettings>();
builder.Services.AddOurAuthentication(authSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

//app.UseAuthorization();

// Upload files 
app.UseStaticFiles();
// add routes
app.MapUserRoutes("User");
app.MapRoleRoutes("Role");
app.MapLocationRoutes("Location");
app.MapEmployeeRoutes("Employee");
app.MapEquipmentRoutes("Equipment");
app.MapActionTypeRoutes("ActionType");
app.MapPermissionRoutes("Permission");
app.MapFeatureRoutes("Feature");
app.MapInventoryRoutes("Inventory");
app.MapProfileRoutes("Profile");
app.MapAuthRoutes("Authentication");
app.MapApiRoutes();

app.UseCors("MyLocalhost");

//Add Authorization
app.UseAuthentication();
app.UseAuthorization();


app.Run();
