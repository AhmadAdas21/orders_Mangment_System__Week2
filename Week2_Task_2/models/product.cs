using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
namespace Week2_Task_2.models
{
    public class product
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public bool active { get; set; } = false;
        public string ksu { get; set; }
        public List<order_item>items { get; set; }
       public int stock { get; set; }

        public product()
        {

        }
        public product ( string name, string description, float price,string k)
        {
           //his.id = id;
            this.name = name;
            this.description = description;
            this.price = price;
         // this.active = active;
            this.ksu = k;
        }
    }
}
