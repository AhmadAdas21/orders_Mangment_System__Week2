using System;
using Microsoft.EntityFrameworkCore;
using Week2_Task_2.models;
using Week2_Task_2.Data;
using Week2_Task_2.Dto.product;

namespace Week2_Task_2.services
{
    public class services : iservices
    {

      /*  public async Task<customer> AddCustomer(customer customer)
        {
            await data_base.Customers.AddAsync(customer);

            await data_base.SaveChangesAsync();

            return customer;
        }*/
        private readonly data_base _data;

        public services(data_base c)
        {
            _data = c;
        }
      

      //public async Task<List<customer>> get_all()
      //{
        //  return await _data.Customers.ToListAsync();
     // }

        public async Task<customer?> get_by_id(int id)
        {
            return await _data.Customers.FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<customer> Create(customer customer)
        {
            await _data.Customers.AddAsync(customer);

            await _data.SaveChangesAsync();

            return customer;
        }

        public async Task<bool> Delete(int id)
        {
            var customer = await _data.Customers.FindAsync(id);

            if (customer == null)
                return false;

            _data.Customers.Remove(customer);

            await _data.SaveChangesAsync();

            return true;
        }



        public async Task<List<product>> get_products(int page, int pageSize, string? search,int? minPrice,int? maxPrice,bool? inStock, string? sortBy,string? sortDirection)
        {

            var query = _data.prod.AsQueryable();
            if(search != null)
            {
                query = query.Where(s => s.name.Contains(search));
            }
            if (minPrice != null)
            {
                query=query.Where(s=>s.price>minPrice);
            }
            if (maxPrice != null)
            {
                query = query.Where(o => o.price < maxPrice);
            }
            if (inStock == true) 
            {
                query = query.Where(d => d.stock>0);
            }
            if (inStock == false)
            {
                query = query.Where(l => l.stock == 0);
            }
            if (sortBy != null)
            {
                if (sortBy == "price")
                {
                    if (sortDirection == "asc")
                    {
                        query = query.OrderBy(p => p.price);
                    }
                    else if (sortDirection == "desc")
                    {
                        query = query.OrderByDescending(p => p.price);
                    }

                }
                if (sortBy == "name")
                {
                    if (sortDirection == "asc")
                    {
                        query = query.OrderBy(p => p.name);
                    }
                    else if(sortDirection == "desc")
                    {
                        query=query.OrderByDescending(p => p.name);
                    }
                }
                

                


            }
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync();

        }
        public async Task<product?> get_product_by_id(int id)
        {
            return await _data.prod.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<product> CreateProduct(add_prod d)
        {
            var product = new product
            {
                name = d.name,
                price = d.price,
                description = d.description,
                ksu = d.ksu,
                stock = d.stock,
                active = d.active
            };

            await _data.prod.AddAsync(product);
            await _data.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateProduct(int id, update_prod d)
        {
            var x =await _data.prod.FirstOrDefaultAsync(x => x.id == id);

            if (x == null)
            {
                return false;
            }

            x.name = d.name;
            x.price = d.price;
            x.description = d.description;
            x.active = d.active;
            x.stock = d.stock;

            await _data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var x =await _data.prod.FirstOrDefaultAsync(x => x.id == id);

            if (x == null)
            {
                return false;
            }

            _data.prod.Remove(x);

            await _data.SaveChangesAsync();

            return true;
        }


    }
}
