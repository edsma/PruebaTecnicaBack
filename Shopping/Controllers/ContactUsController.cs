using Microsoft.AspNetCore.Mvc;
using Shopping.Application;
using Shopping.Application.Services.Orders;
using Shopping.Application.Services.Products;
using Shopping.Dtos.Models.User;

namespace Shopping.Controllers
{
	[ApiController]
	[Route( "api/ContactUs" )]
	public class ContactUsController: ControllerBase
	{
		private readonly IProductService _productService;
		public ContactUsController( IProductService productService )
		{
			this._productService = productService;
		}

		/// <summary>
		/// Send a email with user information.
		/// </summary>
		/// <param name="User">Containing user information
		/// to buy.</param>
		/// <returns>An <see cref="User"/> that represents 
		/// the summary of the user.</returns>
		/// <response code="200">Email send</response>
		/// <response code="400">A email error will be happend</response>
		[HttpPost]
		[ProducesResponseType( StatusCodes.Status201Created, Type = typeof( User ) )]
		[ProducesResponseType( StatusCodes.Status400BadRequest )]
		public IActionResult PostSendEmail( User User )
		{
			try
			{
				this._productService.SendEmail( User );
				return this.Ok();
			}
			catch( BadRequestException ex )
			{
				return this.BadRequest( ex.Message );
			}
		}

		[HttpGet]
		public IActionResult Get()
		{
			return this.Ok();
		}
	}
}
