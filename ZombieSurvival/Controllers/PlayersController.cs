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
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            var players = await _playerService.GetAllAsync();
            return Ok(players);
        }

        // GET: api/Players/5
        [HttpGet("{id}")]  
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _playerService.GetByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            var updated = await _playerService.UpdateAsync(id, player);

            if(!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(CreatePlayerRequest request)
        {
            var created = await _playerService.CreateAsync(request);

            if (created is null)
                return BadRequest("Could not create player. Ensure Los Angeles exists.");
                
            return CreatedAtAction(nameof(GetPlayer), new { id = created.Id }, created);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var deleted = await _playerService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        //GET: api/players/5/inventory
        [HttpGet("{id}/inventory")]
        public async Task<ActionResult<IEnumerable<InventoryItemResponse>>> GetInventory(int id)
        {
            var inventory = await _playerService.GetInventoryAsync(id);

            if (inventory is null)
                return NotFound();

            return Ok(inventory);
        }
    }
}
