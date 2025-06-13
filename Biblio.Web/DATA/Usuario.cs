using System.Security.Cryptography.Pkcs;

namespace Biblio.Web.DATA
{
    public class Usuario
    { 
        public int UsuarioId { get; set; }
        public required string Nombre { get; set; }
        public string? Apellido { get; set; }
        public required string Correo { get; set; }
        public required string Clave { get; set; }
        public int? Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime? FechaMod { get; set; }
        public int? UsuarioMod { get; set; }
        public DateTime? FechaElimino { get; set; }
        public int? UsuarioElimino { get; set; }
        public bool Eliminado { get; set; }
        public bool? Estado { get; set; }

    }
}
