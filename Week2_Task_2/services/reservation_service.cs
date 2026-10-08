using Week2_Task_2.models;
using Week2_Task_2.Data;

using Week2_Task_2.Dto.reservation;
using Microsoft.EntityFrameworkCore;

namespace Week2_Task_2.services
{
    public class reservation_service : iservices_reservation
    {
        private readonly data_base db;
        private readonly ILogger<reservation_service> logger;

        public reservation_service(data_base db, ILogger<reservation_service> logger)
        {
            this.db = db;
            this.logger = logger;
        }
        public async Task<reservartion> Create(add_reservation dto)
        {
            var cus = await db.Customers.FirstOrDefaultAsync(x => x.id == dto.customer_id);
            if (cus == null)
            {
                logger.LogWarning("the customer with {id} not found", dto.customer_id);
                throw new InvalidOperationException("customer does not exist");
            }
            if (dto.items == null)
            {
                logger.LogWarning("the items are null");
                throw new InvalidOperationException("reservation must contain at least one item");
            }
            var reservation = new reservartion
            {
                customer_id = dto.customer_id,
                customer = cus,
                created_at = DateTime.Now,
                expires_at = DateTime.Now.AddMinutes(15),
                status = "Active",
                items = new List<reservation_item>()
            };
            foreach (var i in dto.items)
            {
                var product = await db.prod.FirstOrDefaultAsync(p => p.id == i.product_id);
                if (product == null)
                {
                    logger.LogWarning("the product is null");
                    throw new InvalidOperationException("product does not exist");
                }
                if (product.active == false)
                {
                    logger.LogWarning("reservation creation failed, product {ProductId} is inactive", i.product_id);
                    throw new InvalidOperationException("the product must be active");
                }
                if (i.quantity <= 0)
                {
                    logger.LogWarning("the quantity must be above 0");
                    throw new InvalidOperationException("the quantity must be above 0");
                }
                if (i.quantity > product.stock)
                {
                    logger.LogWarning("the quantity must be below the product stock");
                    throw new InvalidOperationException("the quantity is above the product stock");
                }
                product.stock -= i.quantity;
                logger.LogInformation("the quantity you need is reserved");
                reservation.items.Add(new reservation_item
                {
                    product_id = product.id,
                    product = product,
                    quantity = i.quantity
                });
            }

            await db.reservations.AddAsync(reservation);
            await db.SaveChangesAsync();
            return reservation;
        }
        public async Task<reservartion?> GetById(int id)
        {
            var res = await db.reservations.FirstOrDefaultAsync(x => x.id == id);
            if (res == null)
            {
                logger.LogWarning("the reservation not existing");
                return null;
            }
            else
            {
                logger.LogInformation("the reservation exist");
                return res;
            }

        }
        public async Task<bool> Cancel(int id)
        {
            var res = await db.reservations
                .Include(x => x.items)
                .ThenInclude(x => x.product)
                .FirstOrDefaultAsync(x => x.id == id);

            if (res == null)
            {
                logger.LogWarning("the reservation id with id {id} dosent exist", id);
                return false;
            }
            if (res.status != "Active")
            {
                return false;
            }
            foreach (var i in res.items)
            {
                i.product.stock += i.quantity;
            }
            res.status = "Cancelled";
            await db.SaveChangesAsync();
            return true;
        }
        public async Task<int> ExpireReservations()
        {
            var reservations = await db.reservations.Include(x => x.items).ThenInclude(x => x.product).Where(x =>
                    x.status == "Active" &&x.expires_at <= DateTime.Now).ToListAsync();

            foreach (var res in reservations)
            {
                foreach (var i in res.items)
                {
                    i.product.stock += i.quantity;
                }

                res.status = "expired";

                logger.LogInformation("reservation {id} expired and stock returned", res.id);
            }

            await db.SaveChangesAsync();

            return reservations.Count;
        }
        public async Task<order?> ConvertToOrder(int id)
        {
            await using var transaction =
                await db.Database.BeginTransactionAsync();

            var res = await db.reservations
                .Include(x => x.customer).Include(x => x.items).ThenInclude(x => x.product)
                .FirstOrDefaultAsync(x => x.id == id);

            if (res == null)
            {
                logger.LogWarning( "reservation {id} does not exist",id );

                return null;
            }

            if (res.status == "Converted")
            {
                throw new InvalidOperationException("reservation already converted");
            }

            if (res.status == "Cancelled")
            {
                throw new InvalidOperationException("cancelled reservation cannot be converted");
            }

            if (res.status == "expired")
            {
                throw new InvalidOperationException("expired reservation cannot be converted");
            }

            if (res.expires_at <= DateTime.Now)
            {
                foreach (var i in res.items)
                {
                    i.product.stock += i.quantity;
                }

                res.status = "expired";

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                throw new InvalidOperationException("reservation has expired");
            }

            var order = new order
            {
                customer_id = res.customer_id,
                customer = res.customer,
                status = "Pending",
                created_date = DateTime.Now,
                order_items = new List<order_item>()
            };

            float total = 0;

            foreach (var i in res.items)
            {
                var item = new order_item
                {
                    product_id = i.product_id,
                    product = i.product,
                    quantity = i.quantity,
                    price = i.product.price
                };

                total += i.product.price * i.quantity;

                order.order_items.Add(item);
            }

            order.total = total;

            res.status = "Converted";

            await db.order.AddAsync(order);
            await db.SaveChangesAsync();

            await transaction.CommitAsync();

            logger.LogInformation("reservation {reservationId} converted to order {orderId}",res.id,order.id);

            return order;
        }
        public async Task<List<reservartion>> GetAll()
        {
            return await db.reservations.ToListAsync();
        }


    }
}