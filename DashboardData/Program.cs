using DashboardData.Components;
using DashboardData.Data;
using DashboardData.Models;
using DashboardData.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<ISensorService, SensorService>();

var app = builder.Build();

// Création d'un scope temporaire pour récupérer les services
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    if (!context.Sensors.Any())
    {
        Console.WriteLine("--- Génération de données de test ---");

        // 1. Créer les Emplacements
        var labo = new Location { Name = "Labo", Building = "Bât. A" };
        var usine = new Location { Name = "Usine", Building = "Bât. B" };
        context.Locations.AddRange(labo, usine);

        // 2. Créer les Tags
        var tagCritique = new Tag { Label = "Critique" };
        var tagMaintenance = new Tag { Label = "Maintenance" };
        context.Tags.AddRange(tagCritique, tagMaintenance);
        context.SaveChanges(); // Sauvegarder AVANT pour générer les Id

        // 3. Créer les Capteurs avec relations
        var sonde1 = new SensorData
        {
            Name = "Sonde_Alpha", Value = 25.4,
            LocationId = labo.Id,
            Tags = new List<Tag> { tagCritique }
        };
        var sonde2 = new SensorData
        {
            Name = "Sonde_Beta", Value = 40.2,
            LocationId = usine.Id,
            Tags = new List<Tag> { tagCritique, tagMaintenance }
        };
        context.Sensors.AddRange(sonde1, sonde2);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
