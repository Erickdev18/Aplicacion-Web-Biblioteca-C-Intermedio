﻿namespace Biblio.Web.DATA
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }
        public string? Codigo { get; set; }
        public int? IdEstadoPrestamo { get; set; }
        public int? LibroID { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public DateTime? FechaConfirmacionDevolucion { get; set; }
        public bool EstadoEntregado { get; set; }
        public bool EstadoRecibido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime? FechaMod { get; set; }
        public int? UsuarioMod { get; set; }
        public DateTime? FechaElimino { get; set; }
        public int? UsuarioElimino { get; set; }
        public bool? Eliminado { get; set; }
    }
}
