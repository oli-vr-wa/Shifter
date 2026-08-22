using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shifter.API.Endpoints;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Services.Emails;
using Shifter.Core.Entities.Identity;
using Shifter.Infrastructure.Data;
using Shifter.Infrastructure.Data.Seed;
using Shifter.Infrastructure.Repositories.Core;
using Shifter.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection 
builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(Shifter.Application.AssemblyReference).Assembly,
        typeof(Shifter.Infrastructure.AssemblyReference).Assembly)
    .AddClasses(classes => classes.Where(type =>
        (type.Name.EndsWith("Service") && type.Name != "EmailService") ||
        type.Name.EndsWith("Repository") ||
        type.Name.EndsWith("Handler")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IEmailSender<IdentityUser>, IdentityEmailSender>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 10;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ShifterDbContext>()
.AddDefaultTokenProviders();

// Database & Identity Configuration
builder.Services.AddDbContext<ShifterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Athentication & JWT Configuration
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrEmpty(jwtSecret)) throw new InvalidOperationException("JWT secret is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret))
    };
});

builder.Services.AddAuthorization();

// Add endpoints
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        // Must match your React origin perfectly (no trailing slash)
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowReact");

// Seed tests
//using (var scope = app.Services.CreateScope())
//{
//    var serviceProvider = scope.ServiceProvider;
//    try
//    {
//        await DatabaseSeeder.InitializeAsync(serviceProvider);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
//    }
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();
app.MapAuthEndpoints();
app.MapEmployeesEndpoints();
app.MapSchedulerEndpoints();
app.MapTimesheetServiceEndpoints();

app.Run();


