using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.Dto.reservation
{
    public class add_reservation_item
    {
        [Range(1, 10000)]
        public int product_id { get; set; }

        [Range(1, 10000)]
        public int quantity { get; set; }
    }
}