namespace Shopping.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Shopping.Application;
	using Shopping.Application.Services.Products;
	using Shopping.Domain.Models.Products;

	[ApiController]
	[Route( "api/products" )]
	public class ProductController: ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController( IProductService productService )
		{
			this._productService = productService;
		}

		/// <summary>
		/// Returns the orders created.
		/// </summary>			
		/// <response code="200">Returns the product list.</response>
		[HttpGet( "{pageNumber}/{pageSize}" )]
		[ProducesResponseType( typeof( IEnumerable<Product> ), StatusCodes.Status200OK )]
		public IEnumerable<Product> GetProducts( int pageNumber = 1, int pageSize = 10 )
		{
			return this._productService.GetProducts( pageNumber, pageSize );

		}

		/// <summary>
		/// Returns the product that matches the specified ID.
		/// </summary>
		/// <param name="id">The ID of the product.</param>
		/// <response code="200">The product detail.</response> 
		/// <response code="404">An product with the specified ID does not exist.</response>
		/// <returns>The product detail.</returns>
		[HttpGet( "{id}" )]
		[ProducesResponseType( typeof( IEnumerable<Product> ), StatusCodes.Status200OK )]
		[ProducesResponseType( typeof( string ), StatusCodes.Status404NotFound )]
		[ProducesResponseType( typeof( string ), StatusCodes.Status500InternalServerError )]
		public async Task<IActionResult> GetProductAsync( int id )
		{
			try
			{
				Product product = await this._productService.GetProductAsync( id );
				return this.Ok( product );
			}

			catch( NotFoundException ex )
			{
				return this.NotFound( ex.Message );
			}
		}

		/// <summary>
		/// Creates an product.
		/// </summary>
		/// <param name="product"> containing the product detail.</param>
		/// <returns>An <see cref="Product"/> that represents 
		/// the summary of the product.</returns>
		/// <response code="200">The product is created.</response>
		[HttpPost]
		public Product PostProduct( Product product )
		{
			return this._productService.CreateProduct( product );
		}

		/// <summary>
		/// Updates an product.
		/// </summary>
		/// <param name="product"> containing the product detail.</param>
		/// <returns>An <see cref="Product"/> that represents 
		/// the summary of the product updated.</returns>
		/// <response code="200">The product is updated.</response>
		/// <response code="404">An product with the specified ID does not exist.</response>
		[HttpPut]
		public IActionResult UpdateProduct( Product product )
		{
			try
			{
				Product productToUpdate = this._productService.UpdateProduct( product );
				return this.Ok( productToUpdate );
			}
			catch( NotFoundException ex )
			{
				return this.NotFound( ex.Message );
			}
		}

		/// <summary>
		/// Delete an product.
		/// </summary>
		/// <param name="id"> containing the product id.</param>
		/// <response code="200">The product is deleted.</response>
		/// <response code="404">An product with the specified ID does not exist.</response>
		[HttpDelete( "{id}" )]
		public IActionResult DeleteProduct( int id )
		{
			try
			{
				this._productService.DeleteProduct( id );
				return this.Ok();
			}

			catch( NotFoundException ex )
			{
				return this.NotFound( ex.Message );
			}
		}
	}
}