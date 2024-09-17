using MediatR;

namespace App.Domain.Primitives
{
    public record DomainEvent(Guid id) :INotification
    {
    }
}
