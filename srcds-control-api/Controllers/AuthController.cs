using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using srcds_control_api.Exceptions.User;
using srcds_control_api.Models;
using srcds_control_api.Models.DTOs;
using srcds_control_api.Services;

namespace srcds_control_api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;

        public AuthController(IUserService userService, ILogger<AuthController> logger, IConfiguration config)
        {
            _userService = userService;
            _logger = logger;
            _config = config;
        } 

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] User user)
        {
            try
            {
                if(!user.IsValid())
                {
                    return BadRequest();
                }

                var resUser = await _userService.AuthenticateUser(user);
                _logger.LogInformation($"Successfully logged in user: {user.UserName}");

                return Ok(resUser);
            }
            catch (UserException ex)
            {
                _logger.LogWarning(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in {nameof(AuthController)} : {ex.ToString()}", ex);
                return StatusCode(500, ex.ToString());
            }
        }

        [Authorize(Roles = UserRole.User)]
        [HttpPost]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] PasswordChangeDto dto)
        {
            try
            {
                if (!dto.IsValid())
                {
                    return BadRequest();
                }

                await _userService.ChangePassword(dto);

                _logger.LogInformation($"Successfully changed password for user: {dto.UserName}");

                return Ok();
            }
            catch (UserException ex)
            {
                _logger.LogWarning(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in {nameof(AuthController)} : {ex.ToString()}", ex);
                return StatusCode(500, ex.ToString());
            }
        }

        [Authorize(Roles = UserRole.SuperAdmin)]
        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            try
            {
                if (!user.IsValid())
                {
                    return BadRequest();
                }
                await _userService.CreateUser(user);

                _logger.LogInformation($"Successfully created user: {user.UserName}");

                return Ok();
            }
            catch (UserException ex)
            {
                _logger.LogWarning(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in {nameof(AuthController)} : {ex.ToString()}", ex);
                return StatusCode(500, ex.ToString());
            }
        } 
    }
}   