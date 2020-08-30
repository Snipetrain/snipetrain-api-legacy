using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using srcds_control_api.Models;
using srcds_control_api.Services;

namespace srcds_control_api.Controllers
{
    [Authorize]
    [Route("api/srcds")]
    [ApiController]
    public class SrcdsController : ControllerBase
    {

        private readonly ISrcdsService _srcdsService;
        private readonly ILogger<SrcdsController> _logger;

        public SrcdsController(ISrcdsService srcdsService, ILogger<SrcdsController> logger)
        {
            _srcdsService = srcdsService;
            _logger = logger;
        }

        [Authorize(Roles = UserRole.All)]
        [HttpGet]
        [Route("GetServers")]
        public async Task<IActionResult> GetServers()
        {
            try
            {
                var servers = await _srcdsService.GetServersAsync();
                _logger.LogInformation($"Successfully pulled {servers.Count} servers.");
                return Ok(servers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in {nameof(SrcdsController)} : {ex.ToString()}", ex);
                return StatusCode(500, ex.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetServerStatus")]
        public async Task<IActionResult> GetServerInfo(string host, string port)
        {
            try
            {
                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
                {
                    return BadRequest();
                }

                var res = await _srcdsService.GetServerStatus(host, int.Parse(port));

                _logger.LogInformation($"Successfully pulled Status for <{host}:{port}>.");

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