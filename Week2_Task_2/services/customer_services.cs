using Microsoft.EntityFrameworkCore;
using Week2_Task_2.Data;
using Week2_Task_2.Dto.customer;
using Week2_Task_2.models;

namespace Week2_Task_2.services
{
    public class customer_services
    {
        private readonly data_base db;

        public customer_services(data_base db)
        {
            this.db = db;
        }

        public async Task<List<customer>> GetAll()
        {
            return await db.Customers.ToListAsync();
        }

        public async Task<customer?> GetById(int id)
        {
            return await db.Customers.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<customer> Create(create_customer dto)
        {
            var customer = new customer
            {
                name = dto.name,
                email = dto.email
            };

            await db.Customers.AddAsync(customer);
            await db.SaveChangesAsync();

            return customer;
        }

        public async Task<bool> Update(int id, create_customer dto)
        {
            var customer = await db.Customers.FirstOrDefaultAsync(x => x.id == id);

            if (customer == null)
            {
                return false;
            }

            customer.name = dto.name;
            customer.email = dto.email;

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var customer = await db.Customers.FirstOrDefaultAsync(x => x.id == id);

            if (customer == null)
            {
                return false;
            }

            db.Customers.Remove(customer);

            await db.SaveChangesAsync();

            return true;
        }
    }
}