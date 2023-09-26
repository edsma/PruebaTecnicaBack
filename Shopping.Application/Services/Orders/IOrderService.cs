using Shopping.Domain.Models.Orders;
using Shopping.Domain.Models.Ticket;

namespace Shopping.Application.Services.Orders
{
	public interface IOrderService
	{
		IEnumerable<Order> GetOrders();

		Task<Order> GetOrderAsync( int id );

		Task<Order> CreateOrderAsync( Shopping.Dtos.Models.Orders.Order product );

		Task<Ticket> PostCreatePosition( Dtos.Models.Ticket.Ticket ticket );

		Task UpdatePosition( string position );
	}
}
