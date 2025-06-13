namespace Biblio.Web.DATA
{
    public class Rol
    {
        public int RoldId { get; set; }
        public required string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime? FechaMod { get; set; }
        public int? UsuarioMod { get; set; }
        public DateTime? FechaElimino { get; set; }
        public int? UsuarioElimino { get; set; }
        public bool Eliminado { get; set; }

    }
}
