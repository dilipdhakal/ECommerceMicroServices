using Auth.Application.Services.Interface;
using Auth.Domain.Entities;
using Auth.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Wrapper;

namespace AuthAPI.Controllers
{
    public class AuthController : BaseAPIController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel loginRequest)
        {
            var Data = await _authService.GetLoginToken(loginRequest);
            return Ok(Data);
        }
    }
}
