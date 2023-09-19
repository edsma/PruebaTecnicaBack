using Shopping.Domain.Models.Products;
using Shopping.Dtos.Models.User;

namespace Shopping.Application.Services.Products
{
	public interface IProductService
	{
		IEnumerable<Product> GetProducts( int pagepageNumber = 1, int pageSize = 10);

		Task<Product> GetProductAsync( int id );

		Product CreateProduct( Product product );

		Product UpdateProduct( Product product );

		void SendEmail( User user );

		void DeleteProduct( int id );
	}
}
