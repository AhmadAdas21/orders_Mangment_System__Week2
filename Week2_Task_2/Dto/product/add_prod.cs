using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.Dto.product
{
    public class add_prod
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        
        public string name { get; set; }

        [Required]
        [Range(1, 5000)]
        public float price { get; set; }

        [Required]
        [StringLength(200,MinimumLength =20)]
        public string description { get; set; }

        [Required]
        public string ksu { get; set; }

        [Required]
        [Range(0,100000)]
        public int stock { get; set; }
        [Required]
        public bool active { get; set; }

    }
}
