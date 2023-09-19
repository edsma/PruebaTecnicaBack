using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Shopping.Application.Services.Orders;
using Shopping.Application.Services.Products;
using Shopping.Infrastructure.Data;
using Shopping.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
	options.CustomSchemaIds( type => type.ToString() );
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("origins");

app.UseAuthorization();

app.MapControllers();

app.Run();
