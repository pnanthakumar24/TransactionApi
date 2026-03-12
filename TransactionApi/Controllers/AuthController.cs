using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TransactionApi.DTOs;
using TransactionApi.Models;
using TransactionApi.Repositories;
using TransactionApi.Services;

namespace TransactionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher<User> _hasher;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRepository users, IPasswordHasher<User> hasher, ITokenService tokenService)
        {
            _users = users;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var existing = await _users.GetByUsernameAsync(req.Username);
            if (existing != null) return BadRequest("Username already taken");

            var user = new User { Username = req.Username };
            user.PasswordHash = _hasher.HashPassword(user, req.Password);

            var created = await _users.CreateAsync(user);
            return Ok(new { created.Id, created.Username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var user = await _users.GetByUsernameAsync(req.Username);
            if (user == null) return Unauthorized("Invalid credentials");

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (verify == PasswordVerificationResult.Failed) return Unauthorized("Invalid credentials");

            var token = _tokenService.CreateToken(user.Id, user.Username);
            return Ok(new { token });
        }
    }
}