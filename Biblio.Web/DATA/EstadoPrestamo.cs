namespace Biblio.Web.DATA
{
    public class EstadoPrestamo
    {
        public int IdEstadoPrestamo { get; set; }
        public string? Descripcion { get; set; }
        public bool? Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime? FechaMod { get; set; }
        public int? UsuarioMod { get; set; }
        public int? UsuarioElimino { get; set; }
        public DateTime? FechaElimino { get; set; }
        public bool? Eliminado { get; set; }

    }
}
