using EconomizaAlagoasWeb.Models; // CORREÇÃO: Adicionada esta linha
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EconomizaAlagoasWeb.Data
{
    // Namespace ajustado
    public class EconomizaDbContext : DbContext
    {
        public EconomizaDbContext(DbContextOptions<EconomizaDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProdutoConsultado> ProdutosConsultados { get; set; }
    }
}