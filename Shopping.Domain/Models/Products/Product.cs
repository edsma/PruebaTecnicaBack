namespace Shopping.Domain.Models.Products
{
	using System.ComponentModel.DataAnnotations;
	using System.Runtime.Serialization;
	using System.Text.Json.Serialization;

	public class Product
    {
        public int Id { get; set; }

		[Required]
        public string Name { get; set; }

		[Required]
		public int Inventory { get; set; }

		[Required]
		public bool Enabled { get; set; }

		[Required]
		[Range( 1, int.MaxValue, ErrorMessage = "El campo Min debe ser mayor que 0." )]
		public int Min { get; set; }

		[Required]
		public int Max { get; set; }

		[IgnoreDataMember]
		[JsonIgnore]
        public List<OrderProduct>? OrderProducts { get; set; }
	}
}