using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Shopping.Dtos.Models.Products;

namespace Shopping.Dtos.Models.Orders
{
	[DataContract]
	public class Order
	{
		[Required]
		public DateTime Date { get; set; }

		[Required]
		public string IdType { get; set; }

		[Required]
		public string ClientName { get; set; }

		[Required]
		[MinLength( 1, ErrorMessage = "La lista de productos no puede estar vacía." )]
		public List<OrderProduct> Products { get; set; }
	}
}
