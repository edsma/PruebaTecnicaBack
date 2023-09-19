using Microsoft.EntityFrameworkCore;
using Shopping.Domain.Models.Orders;
using Shopping.Domain.Models.Products;

namespace Shopping.Infrastructure.Data
{
	public interface IShoppingContext
	{
		DbSet<Product> Products { get; set; }
		DbSet<Order> Orders { get; set; }
		DbSet<OrderProduct> OrderProducts { get; set; }

		int SaveChanges();
	}
}
