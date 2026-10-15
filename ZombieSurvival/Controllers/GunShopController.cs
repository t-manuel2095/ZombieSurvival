using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZombieSurvival.Data;
using ZombieSurvival.DTOs;
using ZombieSurvival.Models;
using ZombieSurvival.Services;

namespace ZombieSurvival.Controllers
{
    [Route("api/shop")]
    [ApiController]
    public class GunShopController : ControllerBase
    {
        private readonly IGunShopService _gunshopservice;

        public GunShopController(IGunShopService gunShopService)
        {
            _gunshopservice = gunShopService;
        }

        // GET: /api/shop/guns
        [HttpGet]
        public async Task<ActionResult<SellItemResponse>> SellItemAsync([FromBody] SellItemRequest request)
        {
            try
            {
                var result = await _gunshopservice.SellItemAsync(request.PlayerId, request.ItemId, request.Quantity);

                if (result is null)
                    return BadRequest("Player not found.");

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST: /api/shop/buy — body: playerId + gunId
        [HttpPost("buy")]
        public async Task<ActionResult<BuyGunResponse>> BuyGunAsync([FromBody] BuyGunRequest request)
        {
            try
            {
                var result = await _gunshopservice.BuyGunAsync(request.PlayerId, request.GunId);

                if (result is null)
                    return BadRequest("Player not found.");

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
