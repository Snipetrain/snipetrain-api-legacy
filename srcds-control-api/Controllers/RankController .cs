using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using srcds_control_api.Models;
using srcds_control_api.Services;

namespace srcds_control_api.Controllers
{
    [Authorize]
    [Route("api/rank")]
    [ApiController]
    [EnableCors("Cors-Policy")]
    public class RankController : ControllerBase
    {

        private readonly IRankService _rankService;
        private readonly ILogger<RankController> _logger;

        public RankController(IRankService rankService, ILogger<RankController> logger)
        {
            _rankService = rankService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("players")]
        public async Task<IActionResult> GetPlayersInfo(int perPage, string searchString)
        {
            try
            {
                if (perPage < 1)
                {
                    return BadRequest();
                }

                IEnumerable<Player> res;
                res = (String.IsNullOrEmpty(searchString) ? await _rankService.GetRanks(perPage) : await _rankService.GetRanks(perPage, searchString));

                _logger.LogInformation($"Successfully pulled {res.Count()} players.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in {nameof(SrcdsController)} : {ex.ToString()}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("player")]
        public async Task<IActionResult> GetPlayerInfo(string steamId)
        {
            try
            {
                if (string.IsNullOrEmpty(steamId))
                {
                    return BadRequest();
                }

                var res = await _rankService.GetRank(steamId);

                _logger.LogInformation($"Successfully pulled stats for {steamId}");

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in {nameof(SrcdsController)} : {ex.ToString()}", ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}