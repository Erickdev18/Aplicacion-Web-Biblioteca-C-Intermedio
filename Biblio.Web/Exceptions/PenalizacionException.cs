namespace Biblio.Web.Exceptions
{
    public class PenalizacionException : Exception
    {
        public PenalizacionException(string message) : base(message)
        {
        }
        public PenalizacionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
