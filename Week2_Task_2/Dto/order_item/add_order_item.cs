using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.Dto.order_item
{
    public class add_order_item
    {
        [Required]
        [Range(1, 5000)]
        public int product_id { get; set; }

        [Required]
        [Range(1,2000)]
        public int quantity { get; set; }
    }
}
