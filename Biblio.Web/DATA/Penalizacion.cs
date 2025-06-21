namespace Biblio.Web.DATA

{
    public class Penalizacion
    {
        public int IdPenalizacion { get; set; }
        public int IdPrestamo { get; set; }
        public int IdUsuario { get; set; }
        public int IdLibro { get; set; }
        public DateTime Fecha_Retraso { get; set; }
        public int? Dias_Retraso { get; set; }
        public int? Monto_Penalizacion { get; set; }
        public bool Estado_Penalizacion { get; set; }
        public DateTime Fecha_Pago { get; set; }
        public string? Metodo_Pago { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime? FechaMod { get; set; }
        public int? UsuarioMod { get; set; }
        public int? UsuarioElimino { get; set; }
        public DateTime? FechaElimino { get; set; }
        public bool Eliminado { get; set; }

    }
}
