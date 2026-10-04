using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.Dto.orders
{
    public class update_order
    {
        [Required]
      //public string name { get; set; }
    //  [Required]
        public float total { get; set; }
        [Required]
        public int customer_id { get; set; }
        public string status { get; set; }
    }
}
