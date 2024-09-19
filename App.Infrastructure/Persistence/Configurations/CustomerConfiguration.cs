using App.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Domain.ValueObjects;
namespace App.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .HasConversion(customerId => customerId.Id,//convert TO provider
                                  id => new CustomerId(id)); //convert FROM provider

            builder.Property(c => c.FirstName).HasMaxLength(255);
            builder.Property(c => c.LastName).HasMaxLength(255);
            builder.Property(c => c.Email).HasMaxLength(255);
            builder.Property(c => c.Email)
                   .HasConversion(email => email.Value, value =>  Email.Create(value)!); //в базе уже есть значение, оно не null
            
            builder.Property(c => c.PhoneNumber).HasMaxLength(9);
            builder.Property(c => c.PhoneNumber)
                   .HasConversion(phone => phone.Value, value=> PhoneNumber.Create(value)!);

            builder.OwnsOne(c => c.Address, addressBuilder => {
                addressBuilder.Property(a=>a.Country).HasMaxLength(255);
                addressBuilder.Property(a=>a.Line1).HasMaxLength(255);
                addressBuilder.Property(a=>a.Line2).HasMaxLength(255);
                addressBuilder.Property(a=>a.City).HasMaxLength(255);
                addressBuilder.Property(a=>a.State).HasMaxLength(255);
                addressBuilder.Property(a=>a.ZipCode).HasMaxLength(255);
            });

            builder.Property(c => c.IsActive);

        }
    }
}
