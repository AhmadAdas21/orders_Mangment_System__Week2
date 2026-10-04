using System.ComponentModel.DataAnnotations;

namespace Week2_Task_2.models
{
    public class order_item
    {
        [Key]
        public int id { get; set; }

        public int order_id { get; set; }

        public order order { get; set; } = null!;

        public int product_id { get; set; }

        public product product { get; set; } = null!;

        public int quantity { get; set; }

        public float price { get; set; }
        public order_item(int id, order item,order order, int product_id, product product, int quantity,float price,int order_id) { 
            this.id = id;
            this.product = product;
            this.quantity = quantity;
            this.order = order;
            this.product_id = product_id;
            this.order_id = order_id;


        }
        public order_item()
        {

        }
    }
}
