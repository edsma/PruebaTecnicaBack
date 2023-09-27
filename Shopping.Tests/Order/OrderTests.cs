using Microsoft.EntityFrameworkCore;
using Moq;
using Shopping.Application.Services.Orders;
using Shopping.Application.Services.Products;
using Shopping.Domain.Models.Orders;
using Shopping.Domain.Models.Ticket;
using Shopping.Infrastructure.Data;

public class OrderTests
{
	private readonly Mock<IShoppingContext> _contextMock;
	private readonly Mock<IProductService> _productServiceMock;
	private readonly OrderService _orderService;

	public OrderTests()
	{
		_contextMock = new Mock<IShoppingContext>();
		_productServiceMock = new Mock<IProductService>();
		_orderService = new OrderService( _contextMock.Object, _productServiceMock.Object );
	}

	[Fact]
	public void PostPosition()
	{
		// Arrange
		var expectedOrders = new Shopping.Dtos.Models.Ticket.Ticket
		{
			Date = DateTime.Now,
			FullName = "Perenito Gonzales",
			IdCc = "5698547854",
			Status = "Revisar",
			Sucursal = "Castilla",
			TypeOfProcess = "Consulta"
			
		};

		this._contextMock.Setup( c => c.Tickets.Add( It.IsAny<Ticket>() ) );
		this._contextMock.Setup( c => c.SaveChanges() ).Verifiable();


		// Act
		var actualOrders = _orderService.PostCreatePosition( expectedOrders );

		// Assert
		Assert.Equal( expectedOrders, expectedOrders );
	}

	[Fact]
	public void UpdatePosition()
	{
		

		this._contextMock.Setup( c => c.Tickets.Add( It.IsAny<Ticket>() ) );
		this._contextMock.Setup( c => c.SaveChanges() ).Verifiable();


		// Act
		var actualOrders = _orderService.UpdatePosition( "1234567" );

		// Assert
		Assert.True( actualOrders.IsCompleted);
	}

	[Fact]
	public void GetOrders()
	{
		// Arrange
		var expectedOrders = new List<Order>
		{
			new Order
			{
				Id = 1,
				Date = DateTime.Now,
				IdType = "CC",
				ClientName = "Juan"
			},

			new Order
			{
				Id = 2,
				Date = DateTime.Now,
				IdType = "CC",
				ClientName = "Carlos"
			}
		};

		var ordersDbSetMock = new Mock<DbSet<Order>>();
		ordersDbSetMock.As<IQueryable<Order>>().Setup( m => m.Provider ).Returns( expectedOrders.AsQueryable().Provider );
		ordersDbSetMock.As<IQueryable<Order>>().Setup( m => m.Expression ).Returns( expectedOrders.AsQueryable().Expression );
		ordersDbSetMock.As<IQueryable<Order>>().Setup( m => m.ElementType ).Returns( expectedOrders.AsQueryable().ElementType );
		ordersDbSetMock.As<IQueryable<Order>>().Setup( m => m.GetEnumerator() ).Returns( expectedOrders.AsQueryable().GetEnumerator() );

		_contextMock.Setup( c => c.Orders ).Returns( ordersDbSetMock.Object );

		// Act
		var actualOrders = _orderService.GetOrders();

		// Assert
		Assert.Equal( expectedOrders, actualOrders );
	}
}
