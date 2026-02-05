using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api;

namespace Selu383.SP26.Api.Controllers
{
	[ApiController]
	[Route("api/locations")]
	public class LocationsController : ControllerBase
	{
		private readonly DataContext dataContext;

		public LocationsController(DataContext context)
		{
			dataContext = context;
		}

		[HttpGet]
		public ActionResult<LocationDto> GetLocations()
		{
			var locations = dataContext.Locations.ToList();
			return Ok(locations);
		}

		[HttpGet("{id:int}")]
		public ActionResult<LocationDto> GetLocationById(int id)
		{
			var location = dataContext.Locations.FirstOrDefault(x => x.Id == id);
			if (location == null)
			{
				return NotFound();
			}
			return Ok(location);
		}

		[HttpPost]
		public ActionResult<LocationDto> CreateLocation([FromBody] LocationDto dto)
		{
			if (dto == null)
				return BadRequest("No data provided.");

			if (dto.TableCount < 1)
				return BadRequest("Locations must have at least one table.");

			if (string.IsNullOrWhiteSpace(dto.Name)  || dto.Name.Length > 120)
                return BadRequest("Name between 1 and 120 characters is required.");

			var newLocation = new Location
			{
				Name = dto.Name,
				Address = dto.Address,
				TableCount = dto.TableCount
			};

			dataContext.Locations.Add(newLocation);
			dataContext.SaveChanges();

			return CreatedAtAction(nameof(GetLocationById), new { id = newLocation.Id }, new LocationDto
			{
				Id = newLocation.Id,
				Name = newLocation.Name,
				Address = newLocation.Address,
				TableCount = newLocation.TableCount
			});
		}


		[HttpPut("{id:int}")]
		public ActionResult UpdateLocation(int id, [FromBody] LocationDto dto)
		{

			if (dto == null)
				return BadRequest("Update Information.");

			if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 120)
                return BadRequest("Location name must be between 1 and 120 characters.");

			if (dto.TableCount < 1)
				return BadRequest("The table count cannot be less than 1.");

			var location = dataContext.Locations.FirstOrDefault(x => x.Id == id);
			if (location == null)
				return NotFound("Requested Location Not Found.");

			location.Name = dto.Name;
			location.Address = dto.Address;
			location.TableCount = dto.TableCount;
			dataContext.SaveChanges();
			return Ok(new LocationDto
			{
				Id = location.Id,
				Name = location.Name,
				Address = location.Address,
				TableCount = location.TableCount
			});
		}


		[HttpDelete("{id:int}")]
		public ActionResult DeleteLocation(int id)
		{
			var location = dataContext.Locations.Find(id);
			if (location == null)
			{
				return NotFound("No Location Found");
			}
			dataContext.Remove(location);
			dataContext.SaveChanges();
			return Ok("Location has been successfully removed.");
		}
	}
}