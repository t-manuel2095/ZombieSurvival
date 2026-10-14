using Microsoft.AspNetCore.Mvc;
using ZombieSurvival.DTOs;
using ZombieSurvival.Services;

namespace ZombieSurvival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScavengeController : ControllerBase
    {
        private readonly IScavengerService _scavengerService;

        public ScavengeController(IScavengerService scavengerService)
        {
            _scavengerService = scavengerService;
        }

        // POST /api/Scavenge — body: playerId + locationId (area in current city)
        [HttpPost]
        public async Task<ActionResult<ScavengeResponse>> Scavenge(ScavengeRequest request)
        {
            //Catch any exception I may miss
            try
            {
                var result = await _scavengerService.ScavengeAsync(request);

                if (result is null)
                    return BadRequest("Unable to scavenge. Check player, city, area (same city), and stamina.");

                return Ok(result);
            }
            
            //certain exception messages thrown
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
