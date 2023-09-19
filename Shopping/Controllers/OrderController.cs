namespace Shopping.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Shopping.Application;
	using Shopping.Application.Services.Orders;
	using Shopping.Domain.Models.Orders;

	[ApiController]
	[Route( "api/orders" )]
	public class OrderController: ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController( IOrderService orderService )
		{
			this._orderService = orderService;
		}

		/// <summary>
		/// Returns the orders created.
		/// </summary>			
		/// <response code="200">Returns the order list.</response>
		[HttpGet]
		[ProducesResponseType( StatusCodes.Status200OK, Type = typeof( IEnumerable<Order> ) )]
		public IEnumerable<Order> GetOrders()
		{
			return this._orderService.GetOrders();
		}

		/// <summary>
		/// Returns the order that matches the specified ID.
		/// </summary>
		/// <param name="id">The ID of the order.</param>
		/// <response code="200">The order detail.</response> 
		/// <response code="404">An order with the specified ID does not exist.</response>
		/// <returns>The order detail.</returns>
		[HttpGet( "{id}" )]
		[ProducesResponseType( StatusCodes.Status200OK, Type = typeof( Order ) )]
		[ProducesResponseType( StatusCodes.Status404NotFound )]	
		public async Task<IActionResult> GetOrder( int id )
		{
			try
			{
				Order order = await this._orderService.GetOrderAsync( id );
				return this.Ok( order );
			}
			catch ( NotFoundException ex )
			{
				return this.NotFound(ex.Message );
			}
		}

		/// <summary>
		/// Creates an order using existing products.
		/// </summary>
		/// <param name="order">Order import model containing the products
		/// to buy.</param>
		/// <returns>An <see cref="Order"/> that represents 
		/// the summary of the order.</returns>
		/// <response code="200">The order is created.</response>
		/// <response code="400">The products doesnt met requirements.</response>
		[HttpPost]
		[ProducesResponseType( StatusCodes.Status201Created, Type = typeof( Order ) )]
		[ProducesResponseType( StatusCodes.Status400BadRequest )]
		public async Task<IActionResult> PostOrder( Shopping.Dtos.Models.Orders.Order order )
		{
			try
			{
				Order orderCreated = await this._orderService.CreateOrderAsync( order );
				return this.Ok( orderCreated );
			}
			catch( BadRequestException ex )
			{
				return this.BadRequest( ex.Message );
			}
		}
	}
}
