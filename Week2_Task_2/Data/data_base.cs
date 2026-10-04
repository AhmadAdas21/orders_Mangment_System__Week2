using Microsoft.EntityFrameworkCore;
using Week2_Task_2.Controllers;
using Week2_Task_2.models;
namespace Week2_Task_2.Data
{
    public class data_base : DbContext
    {


        public data_base(DbContextOptions<data_base> options):base(options)
        {



        }
        public DbSet<models.customer> Customers { get; set; }
        public DbSet<models.order> order { get; set; }
        public DbSet<models.product> prod { get; set; }
        public DbSet<models.order_item> oi { get; set; }
        public DbSet<reservartion> reservations { get; set; }
        public DbSet<reservation_item> reservation_items { get; set; }
        // await _context.Customers.ToListAsync();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity <models.customer>().HasIndex(c => c.id).IsUnique();

            modelBuilder.Entity<models.product>().Property(p => p.price);

            modelBuilder.Entity<models.order>().Property(o => o.total);

            modelBuilder.Entity<models.order_item>().Property(oi => oi.price);
                


            modelBuilder.Entity<models.order>().HasOne(o => o.customer).WithMany(c => c.orders).HasForeignKey(o => o.customer_id);


            modelBuilder.Entity<models.order_item>()
                .HasOne(oi => oi.order).WithMany(o => o.order_items).HasForeignKey(oi => oi.order_id);

            modelBuilder.Entity<models.order_item>().HasOne(oi => oi.product).WithMany(p => p.items).HasForeignKey(oi => oi.product_id);
            modelBuilder.Entity<reservartion>().HasOne(r => r.customer).WithMany().HasForeignKey(r => r.customer_id);
            modelBuilder.Entity<reservation_item>() .HasOne(ri => ri.reservartion) .WithMany(r => r.items) .HasForeignKey(ri => ri.reservation_id);

            modelBuilder.Entity<reservation_item>()
                .HasOne(ri => ri.product)
                .WithMany()
                .HasForeignKey(ri => ri.product_id);
        }





    }
}
