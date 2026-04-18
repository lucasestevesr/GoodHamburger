namespace GoodHamburger.Domain.Entities.Base
{
    /// <summary>
    /// Representa violação de regra de negócio do domínio.
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
