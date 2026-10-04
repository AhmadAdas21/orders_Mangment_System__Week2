using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.Dto.reservation
{
    public class add_reservation
    {
        [Range(1, 10000)]
        public int customer_id { get; set; }

        [Required]
        [MinLength(1)]
        public List<add_reservation_item> items { get; set; } = new();
    }
}