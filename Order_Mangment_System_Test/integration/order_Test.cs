using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week2_Task_2.models;
using Week2_Task_2.Controllers;
using Week2_Task_2.Dto;
using Week2_Task_2.Data;
using System.Net.Http.Json;
using Azure;
using System.Net;
using Week2_Task_2.Dto.product;
using Week2_Task_2.services;
using Week2_Task_2.Dto.orders;
using Week2_Task_2.Dto.order_item;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging.Abstractions;


namespace Order_Mangment_System_Test.integration
{

    public class order_Test
    {
        [Fact]
        public async Task create_valid_order_return_Created()
        {
            using var factory = new CustomWebApplicationFactory();

            var client = factory.CreateClient();
            using var scope = factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<data_base>();
            var customer = new customer
            {
                name="ahmad",
                email="ahmad@gmail.com"
            };
            var product = new product
            {
                id=5,
                name="juice",
                description="fresh lemon juice",
                price=3,
                stock=1000,
                ksu="JU-050",
                active=true

            };
            db.Customers.AddAsync(customer);
            db.prod.AddAsync(product);
            await db.SaveChangesAsync();
            var order = new add_order
            {
                customer_id=customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                        product_id = 5,
                        quantity = 100
                    }
                }

            };
            
            var final = await client.PostAsJsonAsync("/api/orders", order);

            var body = await final.Content.ReadAsStringAsync();

            Assert.True( final.StatusCode == HttpStatusCode.Created, $"Expected Created but got {final.StatusCode}. Response: {body}" );




        }
        [Fact]
        public async Task create_order_with_empty_items()
        {
            using var factory = new CustomWebApplicationFactory();

            var client = factory.CreateClient();
            using var scope = factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<data_base>();
            var customer = new customer
            {
                name = "ahmad",
                email = "ahmad@gmail.com"
            };
            var product = new product
            {
                id = 5,
                name = "juice",
                description = "fresh lemon juice",
                price = 3,
                stock = 1000,
                ksu = "JU-050",
                active = true

            };
            db.Customers.AddAsync(customer);
            db.prod.AddAsync(product);
            await db.SaveChangesAsync();
            var order = new add_order
            {
                customer_id = customer.id,
                items = new List<add_order_item>
                {
                    new add_order_item
                    {
                       
                    }
                }
            };
            
            var final = await client.PostAsJsonAsync("/api/orders", order);
            Assert.Equal(HttpStatusCode.BadRequest, final.StatusCode);

        }
        [Fact]
        public async Task get_All_orders_after_creation()
        {
            using var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            var customer = new customer
            {
                name = "Ahmad",
                email = "ahmad@gmail.com"
            };

            var product = new product
            {
                name = "Mouse",
                description = "Wireless mouse for daily office usage",
                price = 20,
                stock = 100,
                ksu = "MO-100",
                active = true
            };
            using var scope = factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<data_base>();
            await db.Customers.AddAsync(customer);
            await db.prod.AddAsync(product);
            await db.SaveChangesAsync();







            var order = new add_order
            {
                customer_id = customer.id,

                items = new List<add_order_item>
                { 
       
            new add_order_item
            {
                product_id = product.id,
                quantity = 2
            }
        }
            };
            var createResponse =await client.PostAsJsonAsync("/api/orders", order);

            Assert.Equal( HttpStatusCode.Created,createResponse.StatusCode);

            var getResponse = await client.GetAsync("/api/orders");

            Assert.Equal(HttpStatusCode.OK,getResponse.StatusCode );








        }




    }
}
