using System.Text.Json;
using System.Text.Json.Serialization;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using salian_api.Config;
using salian_api.Config.Extentions;
using salian_api.Config.Extentions.Hangfire;
using salian_api.Config.Mail;
using salian_api.Config.Permissions;
using salian_api.Config.SMS;
using salian_api.Infrastructure.Data;
using salian_api.Notification;
using salian_api.Response;
using salian_api.Routes;
using salian_api.Seeder;
using salian_api.Services.ActionType;
using salian_api.Services.Auth;
using salian_api.Services.Employee;
using salian_api.Services.Equipment;
using salian_api.Services.Feature;
using salian_api.Services.History;
using salian_api.Services.Inventory;
using salian_api.Services.Location;
using salian_api.Services.Mail;
using salian_api.Services.Me;
using salian_api.Services.Password;
using salian_api.Services.Permission;
using salian_api.Services.Profile;
using salian_api.Services.Role;
using salian_api.Services.Sms;
using salian_api.Services.User;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOurSwagger();

// Can access to loginUser
builder.Services.AddHttpContextAccessor();

// Add Caching
builder.Services.AddMemoryCache();

// history
builder.Services.AddScoped<HistoryInterceptor>(); //TODO:check and uncomment

/* Init Databse */
builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, option) =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        option.AddInterceptors(sp.GetRequiredService<HistoryInterceptor>()); //TODO:check and uncomment
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
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();

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

// Cron job (Hangfire) part 1
builder.Services.AddHangfireConfiguration(
    builder.Configuration.GetConnectionString("DefaultConnection")
);

// Email configuration part1
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped<MailSender>();

//Sms Provider
builder.Services.Configure<KavenegarSettings>(builder.Configuration.GetSection("Kavenegar"));
builder.Services.AddScoped<SmsSender>();

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

// Upload files
app.UseStaticFiles();

// Cron job (Hangfire) part 2
app.MapHangfireDashboard(
    "/jobDashboard",
    new DashboardOptions
    {
        //TODO: uncomment after
        //Authorization = new[] { new HangfireAuthorizationFilter() },
        //IsReadOnlyFunc = (DashboardContext context) => true , // just readOnly can't be edit
    }
);

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
app.MapPasswordRoutes("Password");
app.MapHistoryRoutes("History");
app.MapApiRoutes();
app.MapMyRoutes();

app.UseCors("MyLocalhost");

//Add Authorization
app.UseAuthentication();
app.UseAuthorization();

app.Run();
