namespace Biblio.Web.Exceptions
{
    public class LibrosExceptions : Exception
    {
        public LibrosExceptions(string message) : base(message)
        {
        }
        public LibrosExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}

