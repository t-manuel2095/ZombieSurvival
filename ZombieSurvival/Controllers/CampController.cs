using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using ZombieSurvival.DTOs;
using ZombieSurvival.Services;

namespace ZombieSurvival.Controllers
{
    [Route("api/camp")]
    [ApiController]
    public class CampController : ControllerBase
    {
        private readonly ICampServices _campService;

        public CampController(ICampServices campService)
        {
            _campService = campService;
        }
        //POST /api/camp/end-morning
        [HttpPost("end-morning")]
        public async Task<ActionResult> EndMorningAsync(EndMorningRequest request)
        {
            try
            {
                var ok = await _campService.EndMorningAsync(request.PlayerId);

                if (!ok)
                    return BadRequest("Player not found.");

                return Ok(new { Message = "Morning is over. Time to scavenge.", DayPhase = "Evening" });
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //POST /api/camp/use
        [HttpPost("use")]
        public async Task<ActionResult<UseItemRespone>> UseItemAsync(UseItemRequest request)
        {
            try
            {
                var result = await _campService.UseItemAsync(request.PlayerId, request.ItemId, request.Quantity);

                if (result is null)
                    return BadRequest("Can't use that item. Check player, inventory, and quantity.");

                return Ok(result);
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
