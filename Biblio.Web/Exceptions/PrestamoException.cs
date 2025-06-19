namespace Biblio.Web.Exceptions
{
    public class PrestamoException : Exception
    {
        public PrestamoException(string message) : base(message)
        {
        }
        public PrestamoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
