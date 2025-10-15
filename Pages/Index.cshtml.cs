using EconomizaAlagoasWeb.Models;
using EconomizaAlagoasWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace EconomizaAlagoasWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SefazApiClient _sefazApiClient;
        private readonly ProdutoRepository _produtoRepository;

        [BindProperty]
        public PesquisaViewModel Pesquisa { get; set; } = new();

        public List<SelectListItem> Municipios { get; set; } = new();

        public IndexModel(SefazApiClient sefazApiClient, ProdutoRepository produtoRepository)
        {
            _sefazApiClient = sefazApiClient;
            _produtoRepository = produtoRepository;
        }

        public void OnGet()
        {
            // Prepara a lista de municípios para o formulário
            Pesquisa.Municipios = ObterMunicipiosDeAlagoas();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Repopula a lista caso a página precise ser recarregada com erro
            Pesquisa.Municipios = ObterMunicipiosDeAlagoas();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _sefazApiClient.PesquisarProdutosAsync(Pesquisa);
                var registros = response?.Conteudo ?? new List<Registro>();

                if (registros.Any())
                {
                    await _produtoRepository.SalvarDadosNoBanco(registros, DateTime.UtcNow, "Pesquisa Web");
                }

                // Armazena os resultados em TempData para a próxima página ler
                TempData["ResultadosPesquisa"] = JsonSerializer.Serialize(registros);

                // Redireciona para a página de resultados
                return RedirectToPage("/Resultados");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocorreu um erro ao consultar a API: {ex.Message}");
                return Page();
            }
        }

        private List<SelectListItem> ObterMunicipiosDeAlagoas()
        {
            var municipios = new Dictionary<int, string>
            {
                { 2700102, "AGUA BRANCA" }, { 2700201, "ANADIA" }, { 2700300, "ARAPIRACA" }, { 2700409, "ATALAIA" },
                { 2700508, "BARRA DE SANTO ANTONIO" }, { 2700607, "BARRA DE SAO MIGUEL" }, { 2700706, "BATALHA" },
                { 2700805, "BELEM" }, { 2700904, "BELO MONTE" }, { 2701001, "BOCA DA MATA" }, { 2701100, "BRANQUINHA" },
                { 2701209, "CACIMBINHAS" }, { 2701308, "CAJUEIRO" }, { 2701357, "CAMPESTRE" }, { 2701407, "CAMPO ALEGRE" },
                { 2701506, "CAMPO GRANDE" }, { 2701605, "CANAPI" }, { 2701704, "CAPELA" }, { 2701803, "CARNEIROS" },
                { 2701902, "CHA PRETA" }, { 2702009, "COITE DO NOIA" }, { 2702108, "COLONIA LEOPOLDINA" },
                { 2702207, "COQUEIRO SECO" }, { 2702306, "CORURIPE" }, { 2702355, "CRAIBAS" }, { 2702405, "DELMIRO GOUVEIA" },
                { 2702504, "DOIS RIACHOS" }, { 2702553, "ESTRELA DE ALAGOAS" }, { 2702603, "FEIRA GRANDE" },
                { 2702702, "FELIZ DESERTO" }, { 2702801, "FLEXEIRAS" }, { 2702900, "GIRAU DO PONCIANO" },
                { 2703007, "IBATEGUARA" }, { 2703106, "IGACI" }, { 2703205, "IGREJA NOVA" }, { 2703304, "INHAPI" },
                { 2703403, "JACARE DOS HOMENS" }, { 2703502, "JACUIPE" }, { 2703601, "JAPARATINGA" },
                { 2703700, "JARAMATAIA" }, { 2703759, "JEQUIA DA PRAIA" }, { 2703809, "JOAQUIM GOMES" },
                { 2703908, "JUNDIA" }, { 2704005, "JUNQUEIRO" }, { 2704104, "LAGOA DA CANOA" },
                { 2704203, "LIMOEIRO DE ANADIA" }, { 2704302, "MACEIO" }, { 2704401, "MAJOR ISIDORO" },
                { 2704906, "MAR VERMELHO" }, { 2704500, "MARAGOGI" }, { 2704609, "MARAVILHA" },
                { 2704708, "MARECHAL DEODORO" }, { 2704807, "MARIBONDO" }, { 2705002, "MATA GRANDE" },
                { 2705101, "MATRIZ DE CAMARAGIBE" }, { 2705200, "MESSIAS" }, { 2705309, "MINADOR DO NEGRAO" },
                { 2705408, "MONTEIROPOLIS" }, { 2705507, "MURICI" }, { 2705606, "NOVO LINO" },
                { 2705705, "OLHO D'AGUA DAS FLORES" }, { 2705804, "OLHO D'AGUA DO CASADO" },
                { 2705903, "OLHO D'AGUA GRANDE" }, { 2706000, "OLIVENCA" }, { 2706109, "OURO BRANCO" },
                { 2706208, "PALESTINA" }, { 2706307, "PALMEIRA DOS INDIOS" }, { 2706406, "PAO DE ACUCAR" },
                { 2706422, "PARICONHA" }, { 2706448, "PARIPUEIRA" }, { 2706505, "PASSO DE CAMARAGIBE" },
                { 2706604, "PAULO JACINTO" }, { 2706703, "PENEDO" }, { 2706802, "PIACABUCU" }, { 2706901, "PILAR" },
                { 2707008, "PINDOBA" }, { 2707107, "PIRANHAS" }, { 2707206, "POCO DAS TRINCHEIRAS" },
                { 2707305, "PORTO CALVO" }, { 2707404, "PORTO DE PEDRAS" }, { 2707503, "PORTO REAL DO COLEGIO" },
                { 2707602, "QUEBRANGULO" }, { 2707701, "RIO LARGO" }, { 2707800, "ROTEIRO" },
                { 2707909, "SANTA LUZIA DO NORTE" }, { 2708006, "SANTANA DO IPANEMA" }, { 2708105, "SANTANA DO MUNDAU" },
                { 2708204, "SAO BRAS" }, { 2708303, "SAO JOSE DA LAJE" }, { 2708402, "SAO JOSE DA TAPERA" },
                { 2708501, "SAO LUIS DO QUITUNDE" }, { 2708600, "SAO MIGUEL DOS CAMPOS" },
                { 2708709, "SAO MIGUEL DOS MILAGRES" }, { 2708808, "SAO SEBASTIAO" }, { 2708907, "SATUBA" },
                { 2708956, "SENADOR RUI PALMEIRA" }, { 2709004, "TANQUE D'ARCA" }, { 2709103, "TAQUARana" },
                { 2709152, "TEOTONIO VILELA" }, { 2709202, "TRAIPU" }, { 2709301, "UNIAO DOS PALMARES" },
                { 2709400, "VICOSA" }
            };

            return municipios.Select(m => new SelectListItem { Value = m.Key.ToString(), Text = m.Value })
                             .OrderBy(m => m.Text)
                             .ToList();
        }
    }
}