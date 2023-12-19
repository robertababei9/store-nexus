
namespace Application.ExecutionHelper.Exceptions
{

    public class StoreNexusException : Exception
    {
        public ExceptionCode ExceptionCode { private set; get; } = ExceptionCode.Invalid;

        public StoreNexusException(string message) : base(message)
        {

        }

        public StoreNexusException(string message, Exception innerException, ExceptionCode exceptionCode)
            : base(message, innerException)
        {
            this.ExceptionCode = exceptionCode;
        }

        public StoreNexusException(Type type, string message, Exception innerException, ExceptionCode exceptionCode)
            : this(type.AssemblyQualifiedName + message, innerException, exceptionCode)
        {
        }
    }
}
