using App.Domain.Customers;
using App.Domain.Primitives;
using App.Infrastructure.Persistence.Configurations.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence
{
    //dotnet ef migrations add Initial -c ApplicationDataContext -s App.Api -p App.Infrastructure -o Persistence/Migrations
    //dotnet ef database update -c ApplicationDataContext -s App.Api -p App.Infrastructure 
    public class ApplicationDataContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher; 
        public DbSet<Customer> Customers { get; set; }

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options, IPublisher publisher) : base(options)
        {
            _publisher = publisher?? throw new ArgumentNullException(nameof(publisher));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDataContext).Assembly);
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }

        //IUnitOfWork
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //Можно обработать евенты до или после SaveChangesAsync
            //Если после SaveChangesAsync обработать EventHandlers и если в них будет работа с Бд, то будет второй раз вызов SaveChangesAsync, это нужно избежать
            //Евенты вытаскиваются из кэша EF
            IEnumerable<DomainEvent> events = ChangeTracker.Entries<AggregateRoot>() //Фильтруем изменения только те у кого есть состояние AggregateRoot.//При изменении наших агрегатов, тут получим изменения всех сущностей которые агрегаты
                        .Select(e => e.Entity) //получили сущности агрегатов
                        .Where(e => e.GetDomainEvents().Any()) //только те, в которых были брошены евенты
                        .SelectMany(e=>e.GetDomainEvents()); //достать коллекции коллекций, т.к. на каждом агрегате может быть несколько евентов

            foreach(var @event in events)
            {
                await _publisher.Publish(@event, cancellationToken);
            }

            int result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
