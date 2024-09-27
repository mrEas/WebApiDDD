using App.Domain.Customers;
using App.Domain.ValueObjects;
using App.Infrastructure.Persistence;

namespace App.Api.Extensions
{
    public static class SeedData
    {
        public async static Task SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();

            if (!applicationDbContext.Customers.Any())
            {
                applicationDbContext.Customers.AddRange(
                    new Customer(
                        new CustomerId(Guid.NewGuid()),
                        "Silly", "Goose",
                        Email.Create("silly@goose.com"),
                        PhoneNumber.Create("456-78901"),
                        Address.Create("Foolishland", "Funny St", "Clown Ave", "Giggle City", "Hilarity", "54321"),
                        true
                        ),

                    new Customer(
                        new CustomerId(Guid.NewGuid()),
                        "Marty", "McFly",
                        Email.Create("marty@mcfly.com"),
                        PhoneNumber.Create("101-01010"),
                        Address.Create("Hill Valley", "Peach St", "Flying Car Ave", "California", "Futureland", "2024"),
                        true
                        ),

                    new Customer(
                        new CustomerId(Guid.NewGuid()),
                        "Elmo", "Tickles",
                        Email.Create("elmo@sesame.com"),
                        PhoneNumber.Create("888-88888"),
                        Address.Create("Sesame Street", "Cookie Place", "Big Bird Rd", "NYC", "Playland", "12345"),
                        true
                        ),

                    new Customer(
                        new CustomerId(Guid.NewGuid()),
                        "Dr.", "Who",
                        Email.Create("dr.who@tardis.com"),
                        PhoneNumber.Create("777-77777"),
                        Address.Create("Time and Space", "TARDIS Lane", "Gallifrey", "Space", "Time", "00001"),
                        true
                        )
                    );

                await applicationDbContext.SaveChangesAsync();
            }

        }
    }
}
