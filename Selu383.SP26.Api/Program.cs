using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
	builder.Configuration.GetConnectionString("DataContext")
	));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


//DataContext is registered as a service, we create a scope to get an instance of it, then request it using .GetRequiredService<>()
//We call Migrate to apply any migrations prior to app startup

using (var serviceScope = app.Services.CreateScope())
{
	var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
	dbContext.Database.Migrate();
	if (!dbContext.Locations.Any())
	{

		dbContext.Locations.AddRange(
			new Location
			{
				Name = "Seed One",
				Address = "4739 River Rd",
				TableCount = 10
			},
			new Location
			{
				Name = "Seed Two",
				Address = "270 Sawmill Creek",
				TableCount = 15
			},
			new Location
			{
				Name = "Seed Three",
				Address = "843 Diamond drive",
				TableCount = 20
			}


		);
		dbContext.SaveChanges();
	}

}
app.MapControllers();

app.Run();

//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0
// Hi 383 - this is added so we can test our web project automatically
public partial class Program { }