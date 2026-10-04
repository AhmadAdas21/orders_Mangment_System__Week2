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

namespace Order_Mangment_System_Test.integration
{
    public class Prod_Test 
    {
        [Fact]
        public async Task create_product_returns_Created()
        {
            using var factory =new CustomWebApplicationFactory();

            var client = factory.CreateClient();
            var product = new product
            {
                name="bag",
                description= "A black bag for daily use  ",
                price=150,
                stock=30,
                ksu="B-B-220",
                active=true,


            };
            var final =await client.PostAsJsonAsync( "/api/products", product);
            Assert.Equal( HttpStatusCode.Created, final.StatusCode );
            


        }
        [Fact]
        public async Task create_prod_with_invalid_request()
        {
            using var factory =new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            var product = new add_prod
            {
                name = "A",
                description = "short",
                price = -10,
                stock = -5,
                ksu = "X",
                active = true
            };
            var final = await client.PostAsJsonAsync("/api/products", product);
            Assert.Equal( HttpStatusCode.BadRequest, final.StatusCode);


        }
        [Fact]
        public async Task get_product_after_creation()
        {

            using var factory =new CustomWebApplicationFactory();
            var client = factory.CreateClient();
            var produt = new product
            {
                name = "Keyboard",
                description = " keyboard for daily  use",
                price = 120,
                stock = 25,
                ksu = "KE-115",
                active = true
            };
            var final = await client.PostAsJsonAsync("/api/products", produt);
            Assert.Equal(HttpStatusCode.Created, final.StatusCode );
        }

    }
}
