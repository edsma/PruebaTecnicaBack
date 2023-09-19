using Moq;
using Shopping.Application.Services.Products;
using Shopping.Domain.Models.Products;
using Shopping.Infrastructure.Data;
using Shopping.Infrastructure.Repository;

namespace Shopping.Tests.Products
{
	public class ProductTests
	{
		private readonly ProductService _productService;
		private readonly Mock<IRepositoryProduct> _mockRepository;
		private readonly Mock<IShoppingContext> _context;

		public ProductTests()
		{
			this._mockRepository = new Mock<IRepositoryProduct>();
			this._context = new Mock<IShoppingContext>();
			this._productService = new ProductService( this._context.Object, this._mockRepository.Object );
		}

		[Fact]
		public void GetProducts()
		{
			// Arrange
			int pageNumber = 1;
			int pageSize = 10;
			var expectedProducts = new List<Product>
			{
				new Product
				{
					Id = 1,
					Name = "Producto 1",
					Inventory = 10,
					Enabled = true,
					Min = 1,
					Max = 100
				},
				new Product
				{
					Id = 2,
					Name = "Producto 2",
					Inventory = 5,
					Enabled = true,
					Min = 2,
					Max = 50
				},
			};
			this._mockRepository.Setup( repo => repo.GetProducts( pageNumber, pageSize ) ).Returns( expectedProducts );

			// Act
			var actualProducts = _productService.GetProducts( pageNumber, pageSize );

			// Assert
			Assert.Equal( expectedProducts, actualProducts );
		}

		[Fact]
		public async Task GetProductAsync()
		{
			// Arrange
			int id = 1;

			var expectedProduct = new Product
			{
				Id = id,
				Name = "Producto 1",
				Inventory = 10,
				Enabled = true,
				Min = 1,
				Max = 100
			};

			this._context.Setup( r => r.Products.FindAsync( id ) ).ReturnsAsync( expectedProduct );

			var productService = new ProductService( this._context.Object, this._mockRepository.Object );

			// Act
			var actualProduct = await productService.GetProductAsync( id );

			// Assert
			Assert.Equal( expectedProduct, actualProduct );
		}

		[Fact]
		public void CreateProduct()
		{
			// Arrange
			var product = new Product
			{
				Id = 1,
				Name = "Test Product",
				Inventory = 10,
				Enabled = true,
				Min = 1,
				Max = 100
			};

			this._context.Setup( c => c.Products.Add( It.IsAny<Product>() ) );
			this._context.Setup( c => c.SaveChanges() ).Verifiable();

			// Act
			var createdProduct = _productService.CreateProduct( product );

			// Assert
			Assert.Equal( product, createdProduct );
			this._context.Verify( c => c.Products.Add( It.IsAny<Product>() ), Times.Once );
			this._context.Verify( c => c.SaveChanges(), Times.Once );
		}

		[Fact]
		public void DeleteProduct()
		{
			// Arrange
			int productId = 1;
			var product = new Product { Id = productId };

			this._context.Setup( c => c.Products.Find( productId ) ).Returns( product );
			this._context.Setup( c => c.Products.Remove( product ) );
			this._context.Setup( c => c.SaveChanges() ).Verifiable();

			// Act
			_productService.DeleteProduct( productId );

			// Assert
			this._context.Verify( c => c.Products.Find( productId ), Times.Once );
			this._context.Verify( c => c.Products.Remove( product ), Times.Once );
			this._context.Verify( c => c.SaveChanges(), Times.Once );
		}

		[Fact]
		public void UpdateProduct_ExistingProduct_ReturnsUpdatedProduct()
		{
			// Arrange
			int productId = 1;
			var productToUpdate = new Product
			{
				Id = productId,
				Name = "Old Product",
				Inventory = 10,
				Enabled = true,
				Min = 1,
				Max = 100
			};
			var updatedProduct = new Product
			{
				Id = productId,
				Name = "Updated Product",
				Inventory = 20,
				Enabled = false,
				Min = 5,
				Max = 200
			};

			// Mocking the context
			this._context.Setup( c => c.Products.Find( productId ) ).Returns( productToUpdate );
			this._context.Setup( c => c.SaveChanges() ).Verifiable();

			// Act
			var result = _productService.UpdateProduct( updatedProduct );

			// Assert
			Assert.Equal( updatedProduct.Id, result.Id );
			Assert.Equal( updatedProduct.Name, result.Name );
			Assert.Equal( updatedProduct.Inventory, result.Inventory );
			Assert.Equal( updatedProduct.Enabled, result.Enabled );
			Assert.Equal( updatedProduct.Min, result.Min );
			Assert.Equal( updatedProduct.Max, result.Max );
			this._context.Verify( c => c.Products.Find( productId ), Times.Once );
			this._context.Verify( c => c.SaveChanges(), Times.Once );
		}

	}
}
