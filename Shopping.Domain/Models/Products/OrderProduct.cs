using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Shopping.Domain.Models.Orders;

namespace Shopping.Domain.Models.Products
{
	public class OrderProduct
    {
        public int OrderId { get; set; }

		[IgnoreDataMember]
		[JsonIgnore]
		public Order? Order { get; set; }

        public int ProductId { get; set; }

		[IgnoreDataMember]
		public Product? Product { get; set; }

		public int Quantity { get; set; }
    }
}