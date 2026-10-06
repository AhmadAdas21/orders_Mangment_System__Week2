
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Week2_Task_2.Dto.customer;
using Week2_Task_2.Dto.orders;
using Week2_Task_2.services;
using Week2_Task_2.models;

namespace Week2_Task_2.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class order_controller: ControllerBase
    {
      
        private readonly ILogger<order_controller> logger;
        private readonly order_services service;
     


        public order_controller( order_services service,ILogger<order_controller> logger)
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
           
        
          
            if (dto == null)
            {
                return BadRequest("the form is null");
            }
           

           



            var order =await service.Create(dto);
            logger.LogInformation("order {id} created successfully for customer {customerId}", order.id, order.customer_id );

           
               

            return CreatedAtAction( nameof(get_by_id), new { id = order.id },
                new
                {
                    id = order.id,
                    customer_id = order.customer_id,
                    total = order.total,
                    status = order.status,
                    created_date = order.created_date
                }
            );
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<List<response_order>>>update(int id,[FromBody]add_order dto)
        {
     
            if (id <= 0)
            {
                return BadRequest("the id must be above 0");
            }
            var order = await service.Update(id, dto);
            if (!order)
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
            var order= await service.Delete(id);
            if (!order)
            {
                logger.LogWarning("the order with id {id} not found", id);
                return NotFound("order not found");
            }



           


            logger.LogInformation( "order {id}id deleted successfully", id);

            return NoContent();
        }

    }
}
