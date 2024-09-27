using App.Domain.ValueObjects;

namespace App.Domain.Customers
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(CustomerId id);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
        Task<bool> IsExistAsync(CustomerId id);
        Task<bool> IsExistsByEmailAsync(Email email);
        Task<bool> IsExistsByPhoneAsync(PhoneNumber phone);
    }
}
