
namespace Shopping.Domain.Models.Ticket
{
	public class Ticket
	{
		public int Id { get; set; }

		public string FullName { get; set; }

		public string IdCc { get; set; }

		public DateTime Date { get; set; }

		public string TypeOfProcess { get; set; }

		public string Status { get; set; }

		public string sucursal  { get; set; }
	}
}
