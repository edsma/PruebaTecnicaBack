using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shopping.Dtos.Models.Tokens;

namespace Shopping.Controllers
{
	[ApiController]
	[Route( "api/identity" )]
	public class IdentityController: ControllerBase
	{

		private IConfiguration _config;

		public IdentityController( IConfiguration config )
		{
			_config = config;
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Login( [FromBody] TokenGeneration login )
		{
			IActionResult response = Unauthorized();
			var user = AuthenticateUser( login );

			if( user != null )
			{
				string tokenString = GenerateJSONWebToken( user );
				response = Ok( new { token = tokenString } );
			}

			return response;
		}

		private TokenGeneration AuthenticateUser( TokenGeneration login )
		{
			TokenGeneration user = null;

			if( login.Username == "SoyUnaPrueba" )
			{
				user = new TokenGeneration { Username = "SoyUnaPrueba", email = "test.btest@gmail.com" };
			}
			return user;
		}

		private string GenerateJSONWebToken( TokenGeneration userInfo )
		{
			var securityKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _config["Jwt:Key"] ) );
			var credentials = new SigningCredentials( securityKey, SecurityAlgorithms.HmacSha256 );

			var token = new JwtSecurityToken( _config["Jwt:Issuer"],
			  _config["Jwt:Issuer"],
			  null,
			  expires: DateTime.Now.AddMinutes( 120 ),
			  signingCredentials: credentials );

			return new JwtSecurityTokenHandler().WriteToken( token );
		}
	}
}
