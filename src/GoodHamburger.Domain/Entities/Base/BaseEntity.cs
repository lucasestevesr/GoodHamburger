namespace GoodHamburger.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreationDate { get; set; }
    }
}
