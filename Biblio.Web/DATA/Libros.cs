namespace Biblio.Web.DATA
{
    public class Libros
    {
        public int LibroID { get; set; }
        public required string Titulo { get; set; }
        public required string Autor { get; set; }
        public required string ISBN { get; set; }
        public int? Ejemplares { get; set; }
        public string? Editorial { get; set; }
        public int CategoriaId { get; set; }
        public int? EstadoID { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime? FechaMod { get; set; }
        public int? UsuarioMod { get; set; }
        public DateTime? FechaElimino { get; set; }
        public int? UsuarioElimino { get; set; }
        public bool? Eliminado { get; set; }

    }
}
