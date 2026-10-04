using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.Dto.product
{
    public class update_prod
    {
        [Required]
        public string name { get; set; }
        [Required]
        [Range(1, 5000)]
        public float price { get; set; }
        [Required]
        [StringLength(200,MinimumLength =20)]
        
        public string description { get; set; }
        [Required]
        public bool active { get; set; }
        
        public int stock { get; set; }
    }
}
