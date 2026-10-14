using Microsoft.AspNetCore.Mvc;
using ZombieSurvival.DTOs;
using ZombieSurvival.Services;

namespace ZombieSurvival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly ITravelService _travelService;
        public TravelController(ITravelService travelService)
        {
            _travelService = travelService;
        }
        // POST /api/Travel — move a player to a location
        [HttpPost]
        public async Task<ActionResult<TravelResponse>> Travel(TravelRequest request)
        {
            var result = await _travelService.TravelAsync(request);

            if (result is null)
                return BadRequest("Unable to travel. Check player, location, and stamina.");

            return Ok(result);
        }
    }
}
