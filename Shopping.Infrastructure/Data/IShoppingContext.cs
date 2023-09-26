using Microsoft.EntityFrameworkCore;
using Shopping.Domain.Models.Orders;
using Shopping.Domain.Models.Products;
using Shopping.Domain.Models.Ticket;

namespace Shopping.Infrastructure.Data
{
	public interface IShoppingContext
	{
		DbSet<Product> Products { get; set; }
		DbSet<Order> Orders { get; set; }
		DbSet<OrderProduct> OrderProducts { get; set; }
		DbSet<Ticket> Tickets { get; set; }

		int SaveChanges();
	}
}
