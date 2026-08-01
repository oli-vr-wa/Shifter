using Microsoft.EntityFrameworkCore;
using Shifter.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(Shifter.Application.AssemblyReference).Assembly,
        typeof(Shifter.Infrastructure.AssemblyReference).Assembly)
    .AddClasses(classes => classes.Where(type =>
        type.Name.EndsWith("Service") || 
        type.Name.EndsWith("Repository")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddDbContext<ShifterDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.Run();


