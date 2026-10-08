using Xunit;
using Week2_Task_2;
using Week2_Task_2.models;
using Week2_Task_2.Controllers;
using Week2_Task_2.services;
using Week2_Task_2.Data;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week2_Task_2.Dto.order_item;
using Week2_Task_2.Dto.orders;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
//ing Week2_Task_2.
namespace Order_Mangment_System_Test
{
    public class UnitTest1
    {
        //  private readonly data_base db;
        //  private readonly iservices s;

        [Fact]
        public async Task if_create_order_and_the_customer_not_exixt_return_bad()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();
            var customer = new customer
            {
                name = "abd",
                email = "aaa@gmail.com"

            };
            await db.Customers.AddAsync(customer);
            await db.SaveChangesAsync();
            var prod = new product
            {
                id = 20,
                name = "glass",
                description = "ewewewew",
                price = 20,
                stock = 232,
                active = true,
                ksu = "sd-032"

            };
            await db.prod.AddAsync(prod);
            await db.SaveChangesAsync();
            var order = new add_order
            {
                customer_id = 1000,
                items = new List<add_order_item>
                {
                     new add_order_item
                     {

                    product_id = 20,
                    quantity = 5

                     }

                }

            };

            var service = new order_services(db);

            var controller = new order_controller(service, NullLogger<order_controller>.Instance);
            var oo = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.Create(order));
            db.SaveChangesAsync();
            var bad = oo;
            Assert.Equal("the customer does not exist", bad.Message);







        }
        [Fact]
        public async Task if_quantity_Zero_return_bad()
        {

            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();

            var customer = new customer
            {
                id = 1,
                name = "Ahmad",
                email = "ahmad@gmail.com"
            };

            await db.Customers.AddAsync(customer);

            var product = new product
            {
                id = 11,
                name = "Laptop",
                description = "a nice and good laptop",
                price = 1000,
                ksu = "L1",
                stock = 10,
                active = true
            };

            await db.prod.AddAsync(product);

            await db.SaveChangesAsync();

            var service = new order_services(db);

            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var dto = new add_order
            {
                customer_id = 1,

                items = new List<add_order_item>
        {
            new add_order_item
            {
                product_id = 11,
                quantity = 0
            }
        }
            };

            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.Create(dto));

            var o = result;

            Assert.Equal("the quantity must be above 0", o.Message);


            await connection.CloseAsync();
        }
        [Fact]
        public async Task creare_order_with_qunatity_above_the_Stock()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();
            var customer = new customer
            {
                id = 12,
                name = "shadi",
                email = "shadi@hotmail.com"
            };

            var product = new product
            {
                id = 15,
                name = "apple",
                description = "a red and fresh apple",
                price = 2,
                ksu = "Aa-00",
                stock = 1000,
                active = true
            };

            var service = new order_services(db);

            var controller = new order_controller(service, NullLogger<order_controller>.Instance);
            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);

            await db.SaveChangesAsync();

            var dto = new add_order
            {
                customer_id = 12,

                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 15,
                        quantity = 10001
                    }
                }
            };
            // await db.Saved

            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.Create(dto));
            var c = result;

            Assert.Equal("the quantity of the order is above the stock", c.Message);

            await connection.CloseAsync();
        }
        [Fact]
        public async Task when_Delete_order_return_the_Stock_to_original()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();

            var customer = new customer
            {
                id = 12,
                name = "omar",
                email = "Omar@gmail.com"
            };

            var product = new product
            {
                id = 20,
                name = "banana",
                description = "yummy banana",
                stock = 500,
                ksu = "BAN-20",
                active = true
            };

            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();

            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 20,
                        quantity = 10
                    }
                }
            };


            var d = await controller.Create(order);
            var c = await db.order.FirstOrDefaultAsync();
            var k = await db.prod.FindAsync(20);

            Assert.Equal(490, k.stock);
            var delete = await controller.delete(c.id);
            var after = await db.prod.FirstOrDefaultAsync(x => x.id == 20);
            Assert.Equal(500, after.stock);




            //dawait connection.CloseAsync();
        }
        [Fact]
        public async Task ceate_order_with_inactive_product()
        {

            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();
            var customer = new customer
            {
                name = "SAMI",
                email = "sami@gmail.com"
            };
            var product = new product
            {
                id = 21,
                name = "wheel",
                description = "sss",
                stock = 25,
                price = 100,
                ksu = "zos21",
                active = false
            };
            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();
            var o = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 21,
                        quantity = 10
                    }
                }

            };
            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);
            var kk = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.Create(o));
            var oo = kk;


            Assert.Equal("the product is not active", oo.Message);


        }
        [Fact]
        public async Task create_order_with_empty_items()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();
            var customer = new customer
            {
                name = "ahmad emad",
                email = "ahmadnnnn@gmadil.com"
            };
            await db.Customers.AddAsync(customer);
            await db.SaveChangesAsync();

            var product = new product
            {
                name = "charger",
                description = "samsung phones charger",
                price = 50,
                stock = 100,
                ksu = "ch-302",
                active = true

            };
            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {

                }
            };
            //  await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();
            order_services s = new order_services(db);
            //var con=new Controller()
            var c = new order_controller(s, NullLogger<order_controller>.Instance);
            // await db.order.AddAsync(order);

            var dd = await Assert.ThrowsAsync<InvalidOperationException>(() => c.Create(order));
            var bad = dd;
            Assert.Equal("the order must have at least one item", bad.Message);


        }
        [Fact]
        public async Task create_order_with_negative_quantity()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();
            var customer = new customer
            {
                id = 12,
                name = "shadi",
                email = "shadi@hotmail.com"
            };

            var product = new product
            {
                id = 15,
                name = "apple",
                description = "a red and fresh apple",
                price = 2,
                ksu = "Aa-00",
                stock = 1000,
                active = true
            };

            var service = new order_services(db);

            var controller = new order_controller(service, NullLogger<order_controller>.Instance);
            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);

            await db.SaveChangesAsync();

            var dto = new add_order
            {
                customer_id = 12,

                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 15,
                        quantity = -5
                    }
                }
            };
            // await db.Saved

            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.Create(dto));
            var c = result;
            Assert.Equal("the quantity must be above 0", c.Message);

        }
        [Fact]
        public async Task create_order_decrease_stock()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();

            var customer = new customer
            {
                id = 12,
                name = "omar",
                email = "Omar@gmail.com"
            };

            var product = new product
            {
                id = 20,
                name = "banana",
                description = "yummy banana",
                stock = 500,
                ksu = "BAN-20",
                active = true
            };

            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();

            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 20,
                        quantity = 10
                    }
                }
            };


            var d = await controller.Create(order);
            var after = await db.prod.FindAsync(20);
            await db.SaveChangesAsync();
            Assert.Equal(490, after.stock);

        }
        [Fact]
        public async Task create_calculate_correct_Total()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();

            var customer = new customer
            {
                id = 12,
                name = "omar",
                email = "Omar@gmail.com"
            };

            var product = new product
            {
                id = 20,
                name = "banana",
                description = "yummy banana",
                stock = 500,
                ksu = "BAN-20",
                price = 5,
                active = true
            };

            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();

            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 20,
                        quantity = 10
                    }
                }
            };


            var d = await controller.Create(order);
            var final = await db.order.FirstOrDefaultAsync(x => x.customer_id == customer.id);
            await db.SaveChangesAsync();

            Assert.Equal(50, final.total);


        }
        [Fact]
        public async Task create_order_with_initial_Status_pending()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();

            var customer = new customer
            {
                id = 12,
                name = "omar",
                email = "Omar@gmail.com"
            };

            var product = new product
            {
                id = 20,
                name = "banana",
                description = "yummy banana",
                stock = 500,
                ksu = "BAN-20",
                price = 5,
                active = true
            };

            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();

            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 20,
                        quantity = 10
                    }
                }
            };


            var d = await controller.Create(order);
            var final = await db.order.FirstOrDefaultAsync(x => x.customer_id == customer.id);
            Assert.Equal("Pending", final.status);

        }
        [Fact]
        public async Task create_order_and_the_order_will_exist_in_database()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();

            var customer = new customer
            {
                id = 12,
                name = "omar",
                email = "Omar@gmail.com"
            };

            var product = new product
            {
                id = 20,
                name = "banana",
                description = "yummy banana",
                stock = 500,
                ksu = "BAN-20",
                price = 5,
                active = true
            };

            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();

            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 20,
                        quantity = 10
                    }
                }
            };


            var d = await controller.Create(order);
            var final = await db.order.FirstOrDefaultAsync();
            Assert.NotNull(final);
        }
        [Fact]
        public async Task create_order_and_the_order_will_returned_by_id()

        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();

            var customer = new customer
            {
                id = 12,
                name = "omar",
                email = "Omar@gmail.com"
            };

            var product = new product
            {
                id = 20,
                name = "banana",
                description = "yummy banana",
                stock = 500,
                ksu = "BAN-20",
                price = 5,
                active = true
            };

            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();

            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 20,
                        quantity = 10
                    }
                }
            };


            var d = await controller.Create(order);
            var ord = await db.order.FirstOrDefaultAsync();
            var res = controller.get_by_id(ord.id);

            Assert.NotNull(res);

        }
        [Fact]
        public async Task update_order_status_when_completed()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();
            var customer = new customer
            {
                id = 12,
                name = "omar",
                email = "Omar@gmail.com"
            };

            var product = new product
            {
                id = 20,
                name = "banana",
                description = "yummy banana",
                stock = 500,
                ksu = "BAN-20",
                price = 5,
                active = true
            };

            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();

            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 20,
                        quantity = 10
                    }
                }
            };

            var d = await controller.Create(order);
            var created = await db.order.FirstOrDefaultAsync();
            created.status = "complete";
            await db.SaveChangesAsync();

            var final = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.update(created.id, order));
            var c = final;
            Assert.Equal("you cant modify a complete order", c.Message);

        }
        [Fact]
        public async Task delete_order_that_dosent_exist()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();
            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);
            var res = await controller.delete(100);
            var x = Assert.IsType<NotFoundObjectResult>(res.Result);
            Assert.Equal("order not found", x.Value);
        }
        [Fact]
        public async Task get_order_and_the_order_not_found()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<data_base>().UseSqlite(connection).Options;

            await using var db = new data_base(options);

            await db.Database.EnsureCreatedAsync();
            var service = new order_services(db);
            var controller = new order_controller(service, NullLogger<order_controller>.Instance);

            var result = await controller.get_by_id(1000);

            Assert.IsType<NotFoundResult>(result.Result);

        }
    }
}


