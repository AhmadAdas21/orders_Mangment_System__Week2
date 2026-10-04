using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.models
{
    public class reservartion
    {
        [Key]
        public int id { get; set; }

        public int customer_id { get; set; }

        public customer customer { get; set; }

        public DateTime created_at { get; set; }

        public DateTime expires_at { get; set; }

        public string status { get; set; } = "Active";
        public List<reservation_item> items { get; set; } = new();

    }
}
