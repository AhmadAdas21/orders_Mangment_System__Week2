using Microsoft.EntityFrameworkCore;
using Week2_Task_2.Data;
using Week2_Task_2.Dto.orders;
using Week2_Task_2.models;

namespace Week2_Task_2.services
{
    public class order_services
    {
        private readonly data_base db;

        public order_services(data_base db)
        {
            this.db = db;
        }

        public async Task<List<order>> GetAll()
        {
            return await db.order.ToListAsync();
        }

        public async Task<order?> GetById(int id)
        {
            return await db.order.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<order> Create(add_order dto)
        {
            if (dto.customer_id <= 0)
            {
                throw new InvalidOperationException( "the customer id is invalid");
            }

            var customer = await db.Customers.FirstOrDefaultAsync(x => x.id == dto.customer_id);

            if (customer == null)
            {
                throw new InvalidOperationException("the customer does not exist");
            }

            if (dto.items == null || dto.items.Count == 0)
            {
                throw new InvalidOperationException("the order must have at least one item");
            }

            await using var transaction =await db.Database.BeginTransactionAsync();

            var order = new order
            {
                customer = customer,
                customer_id = dto.customer_id,
                status = "Pending",
                created_date = DateTime.Now,
                order_items = new List<order_item>()
            };

            float total = 0;

            foreach (var item in dto.items)
            {
                var product = await db.prod.FirstOrDefaultAsync(x => x.id == item.product_id);

                if (product == null)
                {
                    throw new InvalidOperationException("product does not exist");
                }

                if (product.active == false)
                {
                    throw new InvalidOperationException("the product is not active");
                }

                if (item.quantity <= 0)
                {
                    throw new InvalidOperationException("the quantity must be above 0");
                }

                if (item.quantity > product.stock)
                {
                    throw new InvalidOperationException("the quantity of the order is above the stock");
                }

                product.stock -= item.quantity;

                var order_item = new order_item
                {
                    product_id = product.id,
                    product = product,
                    quantity = item.quantity,
                    price = product.price
                };

                total += product.price * item.quantity;

                order.order_items.Add(order_item);
            }

            order.total = total;

            await db.order.AddAsync(order);
            await db.SaveChangesAsync();

            await transaction.CommitAsync();

            return order;
        }

        public async Task<bool> Update(int id, add_order dto)
        {
            var order = await db.order.FirstOrDefaultAsync(x => x.id == id);

            if (order == null)
            {
                return false;
            }

            if (order.status == "complete")
            {
                throw new InvalidOperationException("you cant modify a complete order");
            }

            var customer = await db.Customers.FirstOrDefaultAsync(x => x.id == dto.customer_id);

            if (customer == null)
            {
                throw new InvalidOperationException("customer does not exist");
            }

            order.customer_id = dto.customer_id;
            order.customer = customer;

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var order = await db.order.FirstOrDefaultAsync(x => x.id == id);

            if (order == null)
            {
                return false;
            }

            var items = await db.oi.Include(x => x.product).Where(x => x.order_id == id).ToListAsync();

            foreach (var item in items)
            {
                item.product.stock += item.quantity;
            }

            db.order.Remove(order);

            await db.SaveChangesAsync();

            return true;
        }
    }
}