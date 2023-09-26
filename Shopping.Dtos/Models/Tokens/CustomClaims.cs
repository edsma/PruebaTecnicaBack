using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shopping.Dtos.Models.Tokens
{
	public class CustomClaims
	{
		public JsonElement admin { get; set; }
	}
}
