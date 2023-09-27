using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Shopping.Dtos.Models.Ticket
{
	[DataContract]
	public class Ticket
	{
		[Required]
		public string FullName { get; set; }
		[Required]
		public string IdCc { get; set; }
		[Required]
		public DateTime Date { get; set; }
		[Required]
		public string TypeOfProcess { get; set; }

		public string Status { get; set; }

		[Required]
		public string Sucursal { get; set; }

	}
}
