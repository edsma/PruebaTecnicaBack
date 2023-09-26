﻿namespace Shopping.Application.Services.Orders
{
	using Microsoft.EntityFrameworkCore;
	using Products;
	using Shopping.Domain.Models.Orders;
	using Shopping.Domain.Models.Products;
	using Shopping.Domain.Models.Ticket;
	using Shopping.Infrastructure.Data;
	using static Shopping.Domain.Common.Enums;

	public class OrderService: IOrderService
	{
		private readonly IShoppingContext _context;
		private readonly IProductService _productService;

		public OrderService( IShoppingContext context, IProductService productService )
		{
			this._context = context;
			this._productService = productService;
		}

		public IEnumerable<Order> GetOrders()
		{
			try
			{
				return this._context.Orders.Select( o => o ).Include( o => o.OrderProducts ).ThenInclude( op => op.Product );
			}
			catch( DbUpdateException ex )
			{
				throw new Exception( "Un error ocurrió mientras se consultaba las ordenes", ex );
			}
		}

		public async Task<Order> GetOrderAsync( int id )
		{
			var order = await this._context.Orders.Include( o => o.OrderProducts ).ThenInclude( op => op.Product ).FirstOrDefaultAsync( o => o.Id == id );
			
			if ( order != null ) 
			{
				try
				{
					return order;
				}

				catch( DbUpdateException ex )
				{
					throw new Exception( "Un error ocurrió mientras se consultaba la orden", ex );
				}
			}

			else
			{
				throw new NotFoundException( "Orden no encontrada" );
			}
		}

		public async Task<Order> CreateOrderAsync( Shopping.Dtos.Models.Orders.Order order )
		{
			try
			{
				var newOrder = new Order
				{
					Date = DateTime.Now,
					IdType = order.IdType,
					ClientName = order.ClientName,
					OrderProducts = this.GetOrderProducts( order.Products )
				};

				await this.ValidateInventory( newOrder.OrderProducts );

				var orderCreated = this._context.Orders.Add( newOrder ).Entity;

				this._context.SaveChanges();

				await this.UpdateInventory( newOrder.OrderProducts );

				return this._context.Orders.Include( o => o.OrderProducts ).
											ThenInclude( op => op.Product ).
											FirstOrDefaultAsync( o => o.Id == orderCreated.Id ).Result;
			}

			catch( DbUpdateException ex )
			{
				throw new Exception( "Un error ocurrió mientras se creaba la orden", ex );
			}
		}

		private List<OrderProduct> GetOrderProducts( List<Dtos.Models.Orders.OrderProduct> orderProducts )
		{
			var newOrderProducts = new List<OrderProduct>();

			foreach( var orderProduct in orderProducts )
			{
				newOrderProducts.Add( new OrderProduct
				{
					ProductId = orderProduct.ProductId,
					Quantity = orderProduct.Quantity,
				} );
			}

			return newOrderProducts;
		}

		private async Task UpdateInventory( List<OrderProduct> orderProducts )
		{
			foreach( OrderProduct orderProduct in orderProducts.ToList() )
			{
				Product productToUpdate = await this._productService.GetProductAsync( orderProduct.ProductId );

				productToUpdate.Inventory -= orderProduct.Quantity;

				_ = this._productService.UpdateProduct( productToUpdate );
			}
		}

		private async Task ValidateInventory( IEnumerable<OrderProduct> orderProducts )
		{
			foreach( OrderProduct orderProduct in orderProducts )
			{
				Product product = await this._productService.GetProductAsync( orderProduct.ProductId );

				if( product.Inventory < orderProduct.Quantity )
				{
					throw new BadRequestException( $"No hay suficiente {product.Name}" );
				}

				if( orderProduct.Quantity > product.Max || orderProduct.Quantity < product.Min )
				{
					throw new BadRequestException( $"{product.Name} no cumple con el maximo ({product.Max}) o minimo ({product.Min}) permitido" );
				}
			}
		}

		public async Task<Ticket> PostCreatePosition( Dtos.Models.Ticket.Ticket ticket )
		{
			var queryTicket = await _context.Tickets.FirstOrDefaultAsync(x=> x.IdCc.Equals(ticket.IdCc) && x.Date.Equals(ticket.Date));
			if( queryTicket != null )
			{
				throw new BadRequestException( $"Ya tiene un turno asignado para esa misma hora y día" );
			}
			else
			{
				var newTicket = new Ticket
				{
					Date = ticket.Date,
					FullName = ticket.FullName,
					IdCc = ticket.IdCc,
					TypeOfProcess = ticket.TypeOfProcess,
					Status = TypesProcess.Reserva.ToString(),
					sucursal = ticket.Sucursal,
				};
				var ticketCreated = this._context.Tickets.Add( newTicket ).Entity;
				this._context.SaveChanges();
			}
			return queryTicket;
		}

		public async Task UpdatePosition( string position )
		{
			try
			{
				var actualDate = DateTime.UtcNow;
				var queryTicket = await _context.Tickets.FirstOrDefaultAsync( x => x.IdCc.Equals( position ) && actualDate <= x.Date.AddMinutes( 15 ) );
				if( queryTicket != null )
				{
					queryTicket.Status = TypesProcess.Confirmado.ToString();
					this._context.SaveChanges();
				}
				else
				{
					throw new BadRequestException( $"No se encuentran registros para el usuario ingresado" );
				}
			}
			catch( DbUpdateException ex )
			{
				throw new Exception( "Un error ocurrió mientras se actualizaba el turno", ex );
			}
			
		}
	}
}
