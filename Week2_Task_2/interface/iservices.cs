using Week2_Task_2.Dto.customer;
using Week2_Task_2.models;


namespace Week2_Task_2
{
    public interface iservices
    {
      //Task<List<customer>> get_all();

        Task<customer?> get_by_id(int id);

        Task<customer> Create(customer customer);

        

        Task<bool> Delete(int id);
        Task<List<product>> get_products( int page, int pageSize,string? search, int? minPrice, int? maxPrice, bool? inStock, string? sortBy, string? sortDirection);

    }
}