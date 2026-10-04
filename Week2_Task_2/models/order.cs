using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.models
{
    public class order
    {
        [Key]
        public int id { get; set; }

        public int customer_id { get; set; }

        public customer customer { get; set; } 

        public List<order_item> order_items { get; set; } = new();

        public float total { get; set; }

        public string status { get; set; } = "Pending";

        public DateTime created_date { get; set; } = DateTime.Now;
        public order()
        {

        }

        public order(int id,int customer_id,List<order_item> item,float total,customer customer,string status,DateTime now)
        {
            this.id = id;
            this.customer_id = customer_id;
            this.order_items = item;
            this.total = total;
            this.customer = customer;
            this.status = status;
            this.created_date = now;
        }
        

    }
}
