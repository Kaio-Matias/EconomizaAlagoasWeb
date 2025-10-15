using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EconomizaAlagoasWeb.Models
{
    public class PesquisaViewModel
    {
        // --- Critérios de Pesquisa de Produto ---
        [Display(Name = "Descrição do Produto")]
        public string? Descricao { get; set; }

        [Display(Name = "Código de Barras (GTIN)")]
        public string? Gtin { get; set; }

        [Display(Name = "NCM (Apenas com Descrição)")]
        public string? Ncm { get; set; }

        [Display(Name = "GPC (Apenas com Descrição)")]
        public string? Gpc { get; set; }


        // --- Critérios de Pesquisa de Estabelecimento ---
        [Display(Name = "Município de Alagoas")]
        public int? CodigoIBGEMunicipio { get; set; }

        [Display(Name = "CNPJ (Raiz ou Completo)")]
        public string? Cnpj { get; set; }


        // --- Critérios de Geolocalização ---
        [Display(Name = "Sua Latitude")]
        public double? Latitude { get; set; }

        [Display(Name = "Sua Longitude")]
        public double? Longitude { get; set; }

        [Display(Name = "Raio (1 a 15 km)")]
        public int? Raio { get; set; }


        // --- Outros Filtros ---
        [Display(Name = "Buscar nos últimos (dias)")]
        [Range(1, 10, ErrorMessage = "O valor deve ser entre 1 e 10 dias.")]
        public int Dias { get; set; } = 7;

        // --- Lista para popular o Dropdown de Municípios ---
        public List<SelectListItem> Municipios { get; set; } = new List<SelectListItem>();
    }
}