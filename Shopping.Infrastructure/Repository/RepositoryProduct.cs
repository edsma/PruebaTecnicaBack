using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Shopping.Domain.Models.Products;

namespace Shopping.Infrastructure.Repository
{
	public class RepositoryProduct: IRepositoryProduct
	{
		private readonly IDbConnection _connection;

		public RepositoryProduct( IDbConnection dbConnection )
		{
			this._connection = dbConnection;
		}

		public IEnumerable<Product> GetProducts( int pageNumber, int pageSize )
		{
			int offset = ( pageNumber - 1 ) * pageSize;

			string query = $"SELECT * FROM Products ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

			try
			{
				return this._connection.Query<Product>( query, new { Offset = offset, PageSize = pageSize } );
			}
			catch( SqlException ex )
			{
				throw new Exception( "An error occurred while retrieving the products.", ex );
			}
		}
	}
}
