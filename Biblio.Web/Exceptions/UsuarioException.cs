namespace Biblio.Web.Exceptions
{
    public class UsuarioException : Exception
    {
        public UsuarioException(string message) : base(message)
        {
        }
        public UsuarioException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
