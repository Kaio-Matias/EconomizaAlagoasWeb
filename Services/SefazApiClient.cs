using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization; // CORREÇÃO: Adicionada esta linha
using EconomizaAlagoasWeb.Models;     // CORREÇÃO: Adicionada esta linha

namespace EconomizaAlagoasWeb.Services // CORREÇÃO: Namespace ajustado
{
    public class SefazApiClient
    {
        private readonly HttpClient _client;
        private readonly ApiConfig _apiConfig;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Agora "JsonIgnoreCondition" será encontrado
        };

        public SefazApiClient(HttpClient client, ApiConfig apiConfig)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _apiConfig = apiConfig ?? throw new ArgumentNullException(nameof(apiConfig));
        }

        public async Task<PesquisaProdutoResponse?> PesquisarProdutosAsync(PesquisaViewModel filtros)
        {
            var produtoPayload = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(filtros.Gtin))
            {
                produtoPayload["gtin"] = filtros.Gtin;
            }
            else if (!string.IsNullOrEmpty(filtros.Descricao))
            {
                produtoPayload["descricao"] = filtros.Descricao;
                if (!string.IsNullOrEmpty(filtros.Ncm))
                {
                    produtoPayload["ncm"] = filtros.Ncm.Replace(".", "");
                }
                if (!string.IsNullOrEmpty(filtros.Gpc))
                {
                    produtoPayload["gpc"] = filtros.Gpc;
                }
            }

            var estabelecimentoPayload = new Dictionary<string, object>();
            if (filtros.CodigoIBGEMunicipio.HasValue)
            {
                estabelecimentoPayload["municipio"] = new { codigoIBGE = filtros.CodigoIBGEMunicipio.Value };
            }
            else if (!string.IsNullOrEmpty(filtros.Cnpj))
            {
                estabelecimentoPayload["individual"] = new { cnpj = filtros.Cnpj };
            }
            else if (filtros.Latitude.HasValue && filtros.Longitude.HasValue && filtros.Raio.HasValue)
            {
                estabelecimentoPayload["geolocalizacao"] = new
                {
                    latitude = filtros.Latitude.Value,
                    longitude = filtros.Longitude.Value,
                    raio = filtros.Raio.Value
                };
            }

            var payload = new
            {
                produto = produtoPayload.Any() ? produtoPayload : null,
                estabelecimento = estabelecimentoPayload.Any() ? estabelecimentoPayload : null,
                dias = filtros.Dias,
                pagina = 1,
                registrosPorPagina = 5000
            };

            var payloadJson = JsonSerializer.Serialize(payload, _jsonSerializerOptions);
            var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            if (_client.DefaultRequestHeaders.Contains("AppToken"))
            {
                _client.DefaultRequestHeaders.Remove("AppToken");
            }
            _client.DefaultRequestHeaders.Add("AppToken", _apiConfig.AppToken);

            var response = await _client.PostAsync(_apiConfig.BaseUrl + "produto/pesquisa", content);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PesquisaProdutoResponse>(jsonResponse, _jsonSerializerOptions);
        }
    }
}