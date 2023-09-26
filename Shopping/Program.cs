using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Shopping.Application.Services.Orders;
using Shopping.Application.Services.Products;
using Shopping.Infrastructure.Data;
using Shopping.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
	.AddJwtBearer( options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Issuer"],
			IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( builder.Configuration["Jwt:Key"] ) )
		};
	} );
builder.Services.AddMvc();

builder.Services.AddAuthentication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
	options.CustomSchemaIds( type => type.ToString() );
	options.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please insert JWT with Bearer into field",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	} );
	options.AddSecurityRequirement( new OpenApiSecurityRequirement {
   {
	 new OpenApiSecurityScheme
	 {
	   Reference = new OpenApiReference
	   {
		 Type = ReferenceType.SecurityScheme,
		 Id = "Bearer"
	   }
	  },
	  new string[] { }
	}
  } );
} );
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

string connectionString = builder.Configuration.GetConnectionString( "Default" );
builder.Services.AddDbContext<ShoppingContext>( context => context.UseSqlServer( connectionString ) );
builder.Services.AddTransient<IDbConnection>( sp => new SqlConnection( connectionString ) );
builder.Services.AddTransient<IRepositoryProduct, RepositoryProduct>();
builder.Services.AddTransient<IShoppingContext, ShoppingContext>();

builder.Services.AddCors( options =>
{
	options.AddPolicy( name: "origins",
					  builder =>
					  {
						  builder.AllowAnyOrigin();
						  builder.AllowAnyMethod();
						  builder.AllowAnyHeader();
					  } );
} );


var app = builder.Build();



// Configure the HTTP request pipeline.
if( app.Environment.IsDevelopment() )
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.UseCors( "origins" );

app.UseAuthorization();

app.MapControllers();

app.Run();
