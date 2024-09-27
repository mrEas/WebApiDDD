﻿using App.Domain.Customers;
using App.Domain.ValueObjects;
using App.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDataContext _context;
        public CustomerRepository(ApplicationDataContext? context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Customer customer) => _context.Customers.Add(customer);
        public void Update(Customer customer) => _context?.Customers.Update(customer);
        public void Delete(Customer customer) => _context.Customers.Remove(customer);
        public async Task<List<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();
        public async Task<Customer?> GetByIdAsync(CustomerId id) => await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        public async Task<bool> IsExistAsync(CustomerId id) => await _context.Customers.AnyAsync(c => c.Id == id);

        public async Task<bool> IsExistsByEmailAsync(Email email) => await _context.Customers.AnyAsync(c => c.Email.Value == email.Value);
        public async Task<bool> IsExistsByPhoneAsync(PhoneNumber phone) => await _context.Customers.Where(c => c.PhoneNumber.Value == phone.Value).AnyAsync();


    }
}
