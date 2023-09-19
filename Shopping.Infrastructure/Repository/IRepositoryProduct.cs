using Shopping.Domain.Models.Products;

namespace Shopping.Infrastructure.Repository
{
	public interface IRepositoryProduct
	{
		IEnumerable<Product> GetProducts( int pageNumber, int pageSize );
	}
}
