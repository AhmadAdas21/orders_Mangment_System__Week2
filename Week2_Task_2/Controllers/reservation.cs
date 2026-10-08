using Microsoft.AspNetCore.Mvc;
using Week2_Task_2.Dto.reservation;


namespace Week2_Task_2.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class reservation : ControllerBase
    {
        private readonly iservices_reservation service;
        private readonly ILogger<reservation> logger;
      

        public reservation(iservices_reservation service, ILogger<reservation> logger)
        {
            this.service = service;
            this.logger = logger;
          
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] add_reservation dto)
        {
            var reservation = await service.Create(dto);
            logger.LogInformation("reservation with id{id}", reservation.id);
        
            
            return CreatedAtAction(nameof(GetById), new { id = reservation.id },
               new
               {
                   id = reservation.id,
                   customer_id = reservation.customer_id,
                   created_at = reservation.created_at,
                   expires_at = reservation.expires_at,
                   status = reservation.status
               }
           );
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("the id must be above 0");
            }
            var reservation = await service.GetById(id);
            if (reservation == null)
            {
                logger.LogWarning("the reservation with id {id}", id);
                return NotFound();
            }

            return Ok(reservation);
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await service.GetAll();
            if (result.Count == 0)
            {
                logger.LogWarning("theres no reservations yet");
                return NotFound();
            }
            return Ok(result);
        }
    
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            if (id <= 0)
            {
                logger.LogWarning("the id {id} is not valid", id);
                return BadRequest("the id must be above 0");
            }
            var result = await service.GetById(id);
            if (result == null)
            {
                logger.LogWarning("the id{id} is not valid", id);
                return NotFound();
            }
            if (result.status != "Active")
            {
                logger.LogWarning("reservation {id} cannot be cancelled because status is {status}", id, result.status);

                return BadRequest("Reservation cannot be cancelled due to its current status." );
            }
            await service.Cancel(id);

            return NoContent();
        }
        [HttpPost("{id}/convert")]
        public async Task<ActionResult> convert_order(int id)
        {
            if (id <= 0)
            {
                return BadRequest("the id must be above 0");
            }
            var order = await service.ConvertToOrder(id);

            if (order == null)
            {
                logger.LogWarning("reservation {id} not found", id );

                return NotFound();
            }

            return CreatedAtAction(nameof(orderi.get_by_id), nameof(orderi), new { id = order.id },
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
    }
}
