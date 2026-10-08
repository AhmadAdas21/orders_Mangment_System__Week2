using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week2_Task_2.Data;
using Week2_Task_2.Dto.reservation;
using Week2_Task_2.models;

namespace Week2_Task_2.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class reservation : ControllerBase
    {
        private readonly iservices_reservation service;
        private readonly ILogger<reservation> logger;
       // private readonly data_base db;

        public reservation(iservices_reservation service, ILogger<reservation> logger)
        {
            this.service = service;
            this.logger = logger;
           // this.db = db;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] add_reservation dto)
        {
            var reservation = await service.Create(dto);
            logger.LogInformation("reservation with id{id}", reservation.id);
        //    await db.reservations.AddAsync(reservation);
         //   await db.SaveChangesAsync();
            
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
            var res = await service.GetAll();
            if (res.Count == 0)
            {
                logger.LogWarning("theres no reservations yet");
                return NotFound();
            }
            return Ok(res);
        }
     //   [HttpPut("{id}")]
     /*   public async Task<ActionResult> update(int id, [FromBody] add_reservation dto)
        {
            var res = await db.reservations.FirstOrDefaultAsync(x => x.id == id);
            if (res == null)
            {
                logger.LogWarning("the id {id}is not valid", id);
                return NotFound();
            }
            res.customer_id = dto.customer_id;
            res.customer = await db.Customers.FirstOrDefaultAsync(x => x.id == dto.customer_id);
            res.expires_at = DateTime.Now.AddMinutes(15);
            await db.SaveChangesAsync();

            return Ok(res);


        }
     */
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            if (id <= 0)
            {
                logger.LogWarning("the id {id} is not valid", id);
                return BadRequest("the id must be above 0");
            }
            var res = await service.GetById(id);
            if (res == null)
            {
                logger.LogWarning("the id{id} is not valid", id);
                return NotFound();
            }
            if (res.status != "Active")
            {
                logger.LogWarning("reservation {id} cannot be cancelled because status is {status}", id, res.status);

                return BadRequest("Reservation cannot be cancelled due to its current status." );
            }
            await service.Cancel(id);

            return NoContent();
        }
        [HttpPost("{id}/convert")]
        public async Task<ActionResult> convert_order(int id)
        {
            var order = await service.ConvertToOrder(id);

            if (order == null)
            {
                logger.LogWarning("reservation {id} not found", id );

                return NotFound();
            }

            return CreatedAtAction(nameof(order_controller.get_by_id), "orderi",new { id = order.id },
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
