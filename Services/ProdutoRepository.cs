using EconomizaAlagoasWeb.Data;     // CORREÇÃO: Namespace ajustado
using EconomizaAlagoasWeb.Models;  // CORREÇÃO: Adicionada esta linha
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EconomizaAlagoasWeb.Services // CORREÇÃO: Namespace ajustado
{
    public class ProdutoRepository
    {
        private readonly EconomizaDbContext _context;

        public ProdutoRepository(EconomizaDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SalvarDadosNoBanco(List<Registro> registros, DateTime dataConsulta, string tipo)
        {
            var produtosConsultados = new List<ProdutoConsultado>();

            foreach (var registro in registros)
            {
                var produtoConsultado = new ProdutoConsultado
                {
                    Tipo = tipo,
                    DescricaoSefaz = registro.Produto?.DescricaoSefaz ?? "N/A",
                    Gtin = registro.Produto?.Gtin ?? "N/A",
                    Ncm = registro.Produto?.Ncm ?? "N/A",
                    Gpc = registro.Produto?.Gpc ?? "N/A",
                    UnidadeMedida = registro.Produto?.UnidadeMedida ?? "N/A",
                    DataVenda = registro.Produto?.Venda?.DataVenda ?? DateTime.MinValue,
                    ValorDeclarado = registro.Produto?.Venda?.ValorDeclarado ?? 0,
                    ValorVenda = registro.Produto?.Venda?.ValorVenda ?? 0,
                    Cnpj = registro.Estabelecimento?.Cnpj ?? "N/A",
                    RazaoSocial = registro.Estabelecimento?.RazaoSocial ?? "N/A",
                    NomeFantasia = registro.Estabelecimento?.NomeFantasia ?? "N/A",
                    Telefone = registro.Estabelecimento?.Telefone ?? "N/A",
                    NomeLogradouro = registro.Estabelecimento?.Endereco?.NomeLogradouro ?? "N/A",
                    NumeroImovel = registro.Estabelecimento?.Endereco?.NumeroImovel ?? "N/A",
                    Bairro = registro.Estabelecimento?.Endereco?.Bairro ?? "N/A",
                    Cep = registro.Estabelecimento?.Endereco?.Cep ?? "N/A",
                    CodigoIBGE = registro.Estabelecimento?.Endereco?.CodigoIBGE ?? 0,
                    Municipio = registro.Estabelecimento?.Endereco?.Municipio ?? "N/A",
                    DataConsulta = dataConsulta
                };

                produtosConsultados.Add(produtoConsultado);
            }

            if (produtosConsultados.Any())
            {
                await _context.ProdutosConsultados.AddRangeAsync(produtosConsultados);
                await _context.SaveChangesAsync();
            }
        }
    }
}