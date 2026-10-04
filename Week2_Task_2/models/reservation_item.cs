using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.models
{
    public class reservation_item
    {
        [Key]
        public int id { get; set; }

        public int reservation_id { get; set; }
        public product product { get; set; }
        public reservartion reservartion { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
    }
}
