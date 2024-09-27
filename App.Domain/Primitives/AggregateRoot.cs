namespace App.Domain.Primitives
{
    public abstract class AggregateRoot
    {
        private List<DomainEvent> _domainEvents = new();
        public ICollection<DomainEvent> GetDomainEvents() =>_domainEvents; 

        protected void Raise(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
