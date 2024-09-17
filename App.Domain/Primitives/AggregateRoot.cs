using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
