using Microsoft.EntityFrameworkCore;
using UsuariosAPI_ViceriSeidor.src.Models;

namespace UsuariosAPI_ViceriSeidor.src.Data
{
    public class UsuariosContext : DbContext
    {
        public UsuariosContext(DbContextOptions<UsuariosContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
