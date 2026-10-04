using System;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Week2_Task_2.Data;
using Week2_Task_2.Dto.customer;
using Week2_Task_2.services;

namespace Week2_Task_2.Controllers
{
    [ApiController]
    [Route("api/customers")]


    public class customeri: ControllerBase
    {
        private readonly data_base db;
        private readonly iservices _service;
        private readonly ILogger<customeri> logger;
        public customeri(data_base data, iservices service, ILogger<customeri> logger)
        {

            db = data;
            _service = service;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<models.customer>>> GetAll()
        {
            var x = await db.Customers.ToListAsync();
            return Ok(x);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<data_base>>> Get_by_id(int id)
        {

            var x = await db.Customers.FirstOrDefaultAsync(x => x.id == id);
            if (x == null)
            {
                return NotFound();
            }
            return Ok(x);
        }
        [HttpPost]
        public async Task<ActionResult<response_customer>> Create(create_customer dto)
        {
            var customer = new models.customer
            {
                name = dto.name,
                email = dto.email
            };

            await _service.Create(customer);
            logger.LogInformation("the customer created succefully", customer.id);
            // return Ok(customer);
            return CreatedAtAction(nameof(Get_by_id),new { id = customer.id },customer);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<List<response_customer>>> update(int id, create_customer dto)
        {
            var x = await db.Customers.FirstOrDefaultAsync(x => x.id == id);
            if (x == null)
            {
                logger.LogWarning("the customer{id}not founded", id);

                return NotFound();
               

            }
            x.name = dto.name;
            x.email = dto.email;

            await db.SaveChangesAsync();


            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<response_customer>>> delete(int id)
        {
            if (id < 0)
            {
                return BadRequest("id must be greater than 0");
            }
            var x = db.Customers.FirstOrDefault(x => x.id == id);

            bool s = await _service.Delete(id);

            if (!s)
            {
                logger.LogWarning("the customer is not founded", id);
                return NotFound();
               
            }

            return NoContent();








        }
    }
}
