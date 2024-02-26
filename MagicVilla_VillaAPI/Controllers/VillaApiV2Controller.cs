using Azure;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VillaApiV2Controller : ControllerBase
	{
		private readonly AppDbContext _context;

		public VillaApiV2Controller(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<Villa>> GetVillas()
		{
			return Ok(_context.Villa);
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Villa> GetVilla(int id)
		{
			if (id <= 0)
				return BadRequest();

			var villa = _context.Villa.FirstOrDefault(x => x.Id == id);

			if(villa == null)
				return NotFound();

			return Ok(villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public ActionResult<Villa> CreateVilla([FromBody] VillaDTO villaDto)
		{
			if(_context.Villa.Any(x => x.Name == villaDto.Name))
			{
				ModelState.AddModelError("CustomError", "Name must be uniuqe");
				return BadRequest(ModelState);
			}

			if (villaDto.Id != 0 || villaDto == null)
				return BadRequest();

			Villa villa = new Villa
			{
				Name = villaDto.Name,
				Amenity = villaDto.Amenity,
				ImageUrl = villaDto.ImageUrl,
				Occupancy = villaDto.Occupancy,
				Rate = villaDto.Rate,
				Sqft = villaDto.SqFt,
				Details = villaDto.Details,
			};

			_context.Villa.Add(villa);
			_context.SaveChanges();

			return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult DeleteVilla(int id)
		{
			if(id == 0)
				return BadRequest();

			var villa = _context.Villa.FirstOrDefault(x => x.Id == id);

			if (villa == null)
				return NotFound();

			_context.Villa.Remove(villa);
			_context.SaveChanges();

			return NoContent();
		}

		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDto)
		{
			if (id == 0 || id != villaDto.Id)
				return BadRequest();

			//var villa = _context.Villa.FirstOrDefault(x => x.Id == id);

			//if (villa == null)
			//	return NotFound();

			//villa.Name = villaDto.Name;
			//villa.Amenity = villaDto.Amenity;
			//villa.ImageUrl = villaDto.ImageUrl;
			//villa.Occupancy = villaDto.Occupancy;
			//villa.Rate = villaDto.Rate;
			//villa.Sqft = villaDto.SqFt;
			//villa.Details = villaDto.Details;

			Villa villa = new Villa
			{
				Id = villaDto.Id,
				Name = villaDto.Name,
				Amenity = villaDto.Amenity,
				ImageUrl = villaDto.ImageUrl,
				Occupancy = villaDto.Occupancy,
				Rate = villaDto.Rate,
				Sqft = villaDto.SqFt,
				Details = villaDto.Details,
			};

			_context.Villa.Update(villa);
			_context.SaveChanges();

			return NoContent();
		}

		[HttpPatch("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<Villa> patchDocument)
		{
			if (id == 0 || patchDocument == null)
				return BadRequest();

			var villa = _context.Villa.FirstOrDefault(x => x.Id == id);

			patchDocument.ApplyTo(villa, ModelState);
			if(!ModelState.IsValid)
				return BadRequest();

			_context.Villa.Update(villa);
			_context.SaveChanges();

			return NoContent();
		}

		
	}
}
