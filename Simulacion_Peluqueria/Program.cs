using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Simulacion_Peluqueria.Components;
using Simulacion_Peluqueria.DAL;
using Simulacion_Peluqueria.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Obtenemos el ConStr
var ConStr = builder.Configuration.GetConnectionString("ConStr");
// Agregamos el contexto al builder con el ConStr
builder.Services.AddDbContext<Contexto>(Options => Options.UseSqlite(ConStr));

builder.Services.AddScoped<SimulacionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
