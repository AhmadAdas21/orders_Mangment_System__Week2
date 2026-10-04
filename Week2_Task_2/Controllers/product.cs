using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week2_Task_2.Data;
using Week2_Task_2.Dto.customer;
using Week2_Task_2.Dto.product;
using Week2_Task_2.services;
using Week2_Task_2.models;
using System.Drawing.Printing;
using System.Globalization;

namespace Week2_Task_2.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class producti : ControllerBase
    {
        private readonly data_base _data;
        private readonly iservices s;
        private readonly ILogger<producti> logger;
        public producti(iservices service, data_base d, ILogger<producti> logger)
        {
            _data = d;
            s = service;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<product>>>get_all([FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] int? minPrice = null,
    [FromQuery] int? maxPrice = null,
    [FromQuery] bool? inStock = null,
    [FromQuery] string? sortBy = null,
    [FromQuery] string? sortDirection = "asc")
        {
          /*int page = 1;
            int size = 10;
            string?search= null;
            int?min = 0; 
            int?max = int.MaxValue; 
            bool?inStock = false;
            string?sortby = "name"; 
            string?orderby = "asc";
          */

     var d=await s.get_products(page, pageSize, search, minPrice, maxPrice,inStock, sortBy, sortDirection);

            // var c = await _data.prod.ToListAsync();
            return Ok(d);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<response_prod>> get_by_id(int id)
        {
            if (id < 0)
            {
                return NotFound("product not found");
          //      return BadRequest("the Is must be greater than 0or zero");
            }
            var x = await _data.prod.FirstOrDefaultAsync(x => x.id == id);

            if (x == null)
            {
                return BadRequest("invalid id num");
            }
            return Ok(x);
        }

        [HttpPost]
        public async Task<ActionResult<response_prod>> Create([FromBody] add_prod d)
        {
            if (d == null)
            {
                return BadRequest("the form is null");
            }

            var x = new models.product
            {
                name = d.name,
                price = d.price,
                description = d.description,
                ksu = d.ksu,
                stock = d.stock,
                active = d.active
            };
            await _data.prod.AddAsync(x);
            await _data.SaveChangesAsync();
            logger.LogInformation("product {id}created", x.id);

            return CreatedAtAction(nameof(get_by_id), new { id = x.id }, x);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<response_prod>> update(update_prod d, int id)
        {
            if (id < 0)
            {
                return NotFound("product not found");
             //   return BadRequest("the id must be grater than 0");
            }
            var x = await _data.prod.FirstOrDefaultAsync(x => x.id == id);
            if (x == null)
            {
                logger.LogWarning("product {Id} not found", id);
                return NotFound("the product is null");
                
            }
            x.name = d.name;
            x.price = d.price;
            x.description = d.description;
            x.active = d.active;
            x.stock = d.stock;

            await _data.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<response_prod>> Delete(int id)
        {
            if (id < 0)
            {
                return NotFound("product not found");
             //   return BadRequest("must be above 0");
                
            }
          //var uu = await _data.prod.FirstOrDefaultAsync(x => x.id == id);

            var x = await _data.prod.FirstOrDefaultAsync(x => x.id == id);
            if(x == null)
            {
                logger.LogWarning(" the product is not found {id}", id);
                return NotFound("the product is not found");

                
            }
            _data.prod.Remove(x);
            await _data.SaveChangesAsync();
            logger.LogInformation("the product delteed succesfully{id}", id);
            return NoContent();
        }
    }
}
