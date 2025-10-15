using EconomizaAlagoasWeb.Data;     // CORRE��O: Namespace ajustado
using EconomizaAlagoasWeb.Models;   // CORRE��O: Adicionada esta linha
using EconomizaAlagoasWeb.Services; // CORRE��O: Namespace ajustado
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 1. Carrega as configura��es da API (ApiConfig) do appsettings.json
var apiConfig = new ApiConfig();
builder.Configuration.Bind("ApiConfig", apiConfig);
builder.Services.AddSingleton(apiConfig);

// 2. Configura a Connection String e o DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EconomizaDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Configura o HttpClient para ser usado pelo SefazApiClient
builder.Services.AddHttpClient<SefazApiClient>();

// 4. Adiciona o reposit�rio como um servi�o
builder.Services.AddScoped<ProdutoRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();