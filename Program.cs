using EconomizaAlagoasWeb.Data;
using EconomizaAlagoasWeb.Models;
using EconomizaAlagoasWeb.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CORREÇÃO: Configura o projeto para usar Razor Pages em vez de MVC.
builder.Services.AddRazorPages();

// Adiciona suporte para TempData, que usaremos para passar os resultados da pesquisa
builder.Services.AddSession();

// 2. Carrega as configurações da API (ApiConfig) do appsettings.json
var apiConfig = new ApiConfig();
builder.Configuration.Bind("ApiConfig", apiConfig);
builder.Services.AddSingleton(apiConfig);

// 3. Configura a Connection String e o DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EconomizaDbContext>(options =>
    options.UseSqlServer(connectionString));

// 4. Configura o HttpClient e registra os serviços
builder.Services.AddHttpClient<SefazApiClient>();
builder.Services.AddScoped<ProdutoRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Habilita o uso de Session (necessário para TempData)
app.UseSession();

// 5. CORREÇÃO: Mapeia as Razor Pages para as rotas da aplicação
app.MapRazorPages();

app.Run();