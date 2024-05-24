using Microsoft.EntityFrameworkCore;
using UsuariosAPI_ViceriSeidor.src.Models;

namespace UsuariosAPI_ViceriSeidor.src.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
