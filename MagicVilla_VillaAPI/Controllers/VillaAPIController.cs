//using MagicVilla_VillaAPI.Data;
//using MagicVilla_VillaAPI.Logger;
//using MagicVilla_VillaAPI.Models;
//using MagicVilla_VillaAPI.Models.DTO;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;

//namespace MagicVilla_VillaAPI.Controllers
//{
//	[Route("api/[controller]")] 
//	[ApiController]
//	public class VillaAPIController: ControllerBase
//	{
//		private readonly ILogger<VillaAPIController> _logger;

//		//private readonly ILogging _logging;
//        public VillaAPIController(ILogger<VillaAPIController> logger)
//        {
//			_logger = logger;
//        }
//        [HttpGet]
//		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
//		{
//			_logger.LogInformation("Getting all villas");
//			return Ok(VillaStore.VillaList);
//		}

//		[HttpGet("{id:int}", Name = "GetVilla")]
//		//[ProducesResponseType(200)]
//		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
//		[ProducesResponseType(StatusCodes.Status400BadRequest)]
//		[ProducesResponseType(StatusCodes.Status404NotFound)]
//		public ActionResult<VillaDTO> GetVilla(int id)
//		{
//			if (id == 0)
//			{
//				_logger.LogInformation("error");
//				return BadRequest();
//			}
//			var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);

//			if (villa == null)
//				return NotFound();

//			return Ok(villa);
//		}

//		[HttpPost]
//		[ProducesResponseType(StatusCodes.Status201Created)]
//		[ProducesResponseType(StatusCodes.Status400BadRequest)]
//		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
//		public ActionResult<VillaDTO> CreateVillla([FromBody] VillaDTO villa)
//		{
//			//if (!ModelState.IsValid)
//			//	return BadRequest(ModelState);

//			if (VillaStore.VillaList.Any(x => x.Name.Equals(villa.Name, StringComparison.OrdinalIgnoreCase)))
//			{
//				ModelState.AddModelError("CustomError", "This Name already exists");
//				return BadRequest(ModelState);
//			}

//			if(villa == null)
//				return BadRequest(villa);

//			if (villa.Id > 0)
//				return StatusCode(StatusCodes.Status500InternalServerError);

//			villa.Id = VillaStore.VillaList.Max(x => x.Id) + 1;
//			VillaStore.VillaList.Add(villa);

//			return CreatedAtRoute("GetVilla", new {Id = villa.Id}, villa);
//		}

//		[HttpDelete("{id:int}", Name = "DeleteVilla")]
//		[ProducesResponseType(StatusCodes.Status204NoContent)]
//		[ProducesResponseType(StatusCodes.Status400BadRequest)]
//		[ProducesResponseType(StatusCodes.Status404NotFound)]
//		public IActionResult DeleteVilla(int id)
//		{
//			if (id == 0)
//				return BadRequest();

//			var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);

//			if (villa == null)
//				return NotFound();

//			VillaStore.VillaList.Remove(villa);
//			return NoContent();
//		}

//		[HttpPut("{id:int}")]
//		[ProducesResponseType(StatusCodes.Status204NoContent)]
//		[ProducesResponseType(StatusCodes.Status400BadRequest)]
//		public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDto)
//		{
//			if(villaDto == null || id != villaDto.Id)
//				return BadRequest();

//			var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);

//			villa.Name = villaDto.Name;
//			villa.Occupancy = villaDto.Occupancy;
//			villa.SqFt = villaDto.SqFt;

//			return NoContent();
//		}


//		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
//		[ProducesResponseType(StatusCodes.Status204NoContent)]
//		[ProducesResponseType(StatusCodes.Status400BadRequest)]
//		public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDocument)
//		{
//			if (patchDocument == null || id == 0 )
//				return BadRequest();

//			var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);

//			if (villa == null)
//				return NotFound();

//			patchDocument.ApplyTo(villa, ModelState);

//			if(!ModelState.IsValid)
//				return BadRequest();

//			return NoContent();
//		}

//	}
//}
