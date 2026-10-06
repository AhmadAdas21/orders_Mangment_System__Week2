
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
      
        private readonly ILogger<orderi> logger;
        private readonly order_services service;
     


        public orderi( order_services service,ILogger<orderi> logger)
        {
            this.service = service;
            this.logger = logger;
        }

     
        [HttpGet]
        public async Task<ActionResult<List<order>>> GetAll()
        {

         
            var x = await service.GetAll();
            return Ok(x); 
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<order>>>get_by_id(int id)
        {
          
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

        
          
            if (dto == null)
            {
                return BadRequest("the form is null");
            }
            

          
           var x=await service.Create(dto);
            logger.LogInformation("order {id} created successfully for customer {customerId}",x.id, x.customer_id );

           
               

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

     
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<response_order>>> delete(int id)
        {
      
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



           


            logger.LogInformation( "order {id}id deleted successfully", id);

            return NoContent();
        }

    }
}
