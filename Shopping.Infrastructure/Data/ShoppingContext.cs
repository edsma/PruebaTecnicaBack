namespace Shopping.Infrastructure.Data
{
	using Microsoft.EntityFrameworkCore;
	using Shopping.Domain.Models.Orders;
	using Shopping.Domain.Models.Products;

	public class ShoppingContext: DbContext, IShoppingContext
	{
		public ShoppingContext()
		{
				
		}

		public ShoppingContext( DbContextOptions<ShoppingContext> options ) : base( options ) { }

		public DbSet<Product> Products { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderProduct> OrderProducts { get; set; }

		protected override void OnModelCreating( ModelBuilder modelBuilder )
		{
			modelBuilder.Entity<OrderProduct>().HasKey( x => new { x.OrderId, x.ProductId } );

			modelBuilder.Entity<OrderProduct>()
				.HasOne( po => po.Product )
				.WithMany( p => p.OrderProducts )
				.HasForeignKey( po => po.ProductId );

			modelBuilder.Entity<OrderProduct>()
				.HasOne( po => po.Order )
				.WithMany( o => o.OrderProducts )
				.HasForeignKey( po => po.OrderId );
		}
	}
}