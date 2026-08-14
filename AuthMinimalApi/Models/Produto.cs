using Microsoft.EntityFrameworkCore;

namespace AuthMinimalApi.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public decimal Preco { get; set; }
    }

    public class ProdutoContext : DbContext
    {
        public ProdutoContext(DbContextOptions<ProdutoContext> options) : base(options) { }
        public DbSet<Produto> Produtos { get; set; }
    }
}
