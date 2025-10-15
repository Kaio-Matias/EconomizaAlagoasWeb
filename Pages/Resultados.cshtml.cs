using EconomizaAlagoasWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace EconomizaAlagoasWeb.Pages
{
    public class ResultadosModel : PageModel
    {
        public List<Registro> Resultados { get; set; } = new List<Registro>();

        public IActionResult OnGet()
        {
            if (TempData["ResultadosPesquisa"] is string resultadosJson)
            {
                Resultados = JsonSerializer.Deserialize<List<Registro>>(resultadosJson) ?? new List<Registro>();
                TempData.Keep("ResultadosPesquisa"); // Mantém o dado para o caso de refresh
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }
}