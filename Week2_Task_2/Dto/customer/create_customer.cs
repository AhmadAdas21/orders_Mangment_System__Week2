using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel.DataAnnotations;
namespace Week2_Task_2.Dto.customer
{
    public class create_customer
    {
        [Required]
        public string name { get; set; }
        [Required]
        [EmailAddress]

        public string email { get; set; }
    }
}
