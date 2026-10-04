using System.ComponentModel.DataAnnotations;
using Week2_Task_2.models;
using Week2_Task_2.Dto.order_item;

namespace Week2_Task_2.Dto.orders
{
    public class add_order
    {
        //  public string name { get; set; }
        //  order s = new order(int id ,);
        [Required]
        public int customer_id { get; set; }

        [Required]
        public List<add_order_item> items { get; set; }
        //  [Required]

    }
}
