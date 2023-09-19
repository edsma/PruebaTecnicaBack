using Shopping.Domain.Models.Orders;

namespace Shopping.Application.Services.Orders
{
	public interface IOrderService
	{
		IEnumerable<Order> GetOrders();

		Task<Order> GetOrderAsync( int id );

		Task<Order> CreateOrderAsync( Shopping.Dtos.Models.Orders.Order product );
	}
}
