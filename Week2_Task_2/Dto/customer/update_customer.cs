using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.Dto.customer
{
    public class update_customer
    {
        [Required]
        public string name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }


    }
}
