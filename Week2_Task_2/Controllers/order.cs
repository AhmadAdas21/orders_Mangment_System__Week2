
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week2_Task_2.Data;
using Week2_Task_2.Dto.customer;
using Week2_Task_2.Dto.orders;
using Week2_Task_2.services;
using Week2_Task_2.models;

namespace Week2_Task_2.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class orderi: ControllerBase
    {
      //  private readonly data_base dp;
       // private readonly iservices se;
        private readonly ILogger<orderi> logger;
        private readonly order_services service;
     

        public orderi( order_services service,ILogger<orderi> logger)
        {
            this.service = service;
            this.logger = logger;
        }

     /*   public orderi(data_base dp, iservices se, ILogger<orderi> logger)
        {
       //     this.dp = dp;
            this.se = se;
            this.logger = logger;
        }
     */
        [HttpGet]
        public async Task<ActionResult<List<order>>> GetAll()
        {

            //    var x = await dp.order.ToListAsync();
            var x = await service.GetAll();
            return Ok(x); 
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<order>>>get_by_id(int id)
        {
            //   var x = await dp.order.FirstOrDefaultAsync(x => x.id == id);
            if (id <= 0)
            {
                logger.LogWarning("the order with id {id} is invalid", id);
                return BadRequest("the id must be above 0");

            }
            var x = await service.GetById(id);


            if (x == null)
            {
                return NotFound();
            }

            return Ok(x);
        }
        [HttpPost]
        public async Task<ActionResult<response_order>> Create([FromBody] add_order dto)
        {
            logger.LogInformation("creating order for customer customerid", dto.customer_id);

          //  await using var tt = await dp.Database.BeginTransactionAsync();
         // await using var tt=await dp.Database.BeginTransactionAsync();
            float total = 0;
            if (dto == null)
            {
                return BadRequest("the form is null");
            }
            

          /*  if (dto.customer_id < 0 || await dp.Customers.FirstOrDefaultAsync(x => x.id == dto.customer_id) == null)
            {
                logger.LogWarning("order creation failed,,customer customerid{id} not found", dto.customer_id);
                return BadRequest("the customer id is invalid or the customer does not exist");
                
            }
            
            */
           // var x = await dp.Customers.FirstOrDefaultAsync(x => x.id == dto.customer_id);
           var x=await service.Create(dto);
            logger.LogInformation("order {id} created successfully for customer {customerId}",x.id, x.customer_id );

            /*   var order = new models.order
               {
                   customer = x,
                   customer_id = dto.customer_id,
                   status = "Pending",
                   created_date = DateTime.Now,
                   order_items = new List<models.order_item>()
               };

               if (dto.items == null || dto.items.Count == 0)
               {

                   return BadRequest( "the order must have at least one item" );
               }
               foreach (var o in dto.items)
               {
                   var p= await dp.prod.FirstOrDefaultAsync(p => p.id == o.product_id);
                // var oii= await dp.oi.FirstOrDefaultAsync(x =>x.product_id == p.id);

                   if (p == null)
                   {
                       logger.LogWarning("order creation failed. product productid{id} not found", o.product_id);
                       return BadRequest( "product  does not exist" );

                   }
                   if(p.active == false)
                   {
                       return BadRequest("the product is not active ");
                   }
                   if (p.stock < o.quantity)
                   {
                       return BadRequest("the quatity of the order is above the stock ");
                   }
                   if(o.quantity <= 0)
                   {
                       return BadRequest("the quantity must be above 0");
                   }
                  //ar pro = await dp.prod.FirstOrDefaultAsync(x => x.id == k.product_id);
                   p.stock -= o.quantity;


                   var oi= new models.order_item
                   {
                       product_id = p.id,
                       product = p,
                       quantity = o.quantity,
                       price = p.price
                   };

                   total += p.price * o.quantity;
                   order.order_items.Add(oi);

               }
            */

            // var s = await dp.order.FirstOrDefaultAsync(x => x.customer_id == dto.customer_id);


            //   var d = await dp.oi.FirstOrDefaultAsync(x => x.order_id == s.id);
            //var prod = await dp.prod.FirstOrDefaultAsync(x => x.id == d.product_id);







            //  if (order.order_items == null)
            //{
            //  return BadRequest("the order item is null");
            //  }




            //  prod.stock-=d.quantity;

            //    logger.LogInformation("order orderid created successfully for customer custom{id}",  order.customer_id);


            return CreatedAtAction( nameof(get_by_id), new { id = x.id },
                new
                {
                    id = x.id,
                    customer_id = x.customer_id,
                    total = x.total,
                    status = x.status,
                    created_date = x.created_date
                }
            );
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<List<response_order>>>update(int id,[FromBody]add_order dto)
        {
       //     var x=await dp.order.FirstOrDefaultAsync(x=>x.id==id);
            if (id < 0)
            {
                return BadRequest("the id must be above 0");
            }
            var x = await service.Update(id, dto);
            if (!x)
            {
                logger.LogWarning("the order with id {id} not found", id);
                return NotFound("order not found");
            }



            //  x.customer_id=dto.customer_id;

            //x.status = dto.status;

            //     await dp.SaveChangesAsync();


            return NoContent();
            //validate(dto);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<response_order>>> delete(int id)
        {
        //    var x = await dp.order.FirstOrDefaultAsync(x => x.id == id);
            if (id <= 0)
            {
                return BadRequest("must be above 0");
            }
            var x= await service.Delete(id);
            if (!x)
            {
                logger.LogWarning("the order with id {id} not found", id);
                return NotFound("order not found");
            }



            //  var s = await dp.oi.Where(x => x.order_id == id).ToListAsync();  


            logger.LogInformation( "order {id}id deleted successfully", id);

            return NoContent();
        }

    }
}
