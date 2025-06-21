namespace Biblio.Web.Exceptions
{
    public class EstadoPrestamoException : Exception
    {
        public EstadoPrestamoException(string message) : base(message)
        {
        }
        public EstadoPrestamoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
