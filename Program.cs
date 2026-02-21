using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using salian_api.Config;
using salian_api.Config.Extentions;
using salian_api.Config.Permissions;
using salian_api.Infrastructure.Data;
using salian_api.Infrastructure.Interceptors;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Routes;
using salian_api.Seeder;
using salian_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOurSwagger();


// Can access to loginUser
builder.Services.AddHttpContextAccessor();
// history
builder.Services.AddScoped<HistoryInterceptor>();

/* Init Databse */
builder.Services.AddDbContext<ApplicationDbContext>((sp, option) =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    option.AddInterceptors(sp.GetRequiredService<HistoryInterceptor>());
}
);


/* For customize error message */
builder.Services.AddLogging();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(option =>
{
    option.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
    );
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
builder.Services.AddScoped<IMeService, MeService>();

//Add CORS
builder.Services.AddCors(options =>
    options.AddPolicy(
        "MyLocalhost",
        builder =>
            builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:3000", "http://localhost:5005")
    )
);

// Add new configuration
var authConfiguration = builder.Configuration.GetSection("AuthSettings");
builder.Services.Configure<AuthSettings>(authConfiguration);

// scafold Jwt Authorization
var authSettings = authConfiguration.Get<AuthSettings>();
builder.Services.AddOurAuthentication(authSettings);

// Email configuration part1
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();


// Add seeder part1
builder.Services.AddScoped<ISeeder, RoleSeeder>();
builder.Services.AddScoped<ISeeder, UserSeeder>();
builder.Services.AddScoped<ISeeder, PermissionSeeder>();
builder.Services.AddScoped<SeederProvider>();


// overwrite and create custome permissions
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

var app = builder.Build();

// Add seeder part2
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<SeederProvider>();
    await seeder.SeedAllAsync(db, scope.ServiceProvider);
}

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
app.MapMyRoutes();

app.UseCors("MyLocalhost");

//Add Authorization
app.UseAuthentication();
app.UseAuthorization();

app.Run();
