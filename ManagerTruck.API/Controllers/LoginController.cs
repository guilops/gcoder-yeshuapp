using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ManagerTruck.Dtos;

namespace ManagerTruck.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("auth/")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public LoginController(ILogger<LoginController> logger, UserManager<IdentityUser> userManager,
                               IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Senha))
                return Unauthorized("Credenciais inv�lidas");

            var token = GerarTokenJwt(user);

            if (string.IsNullOrEmpty(token))
                return StatusCode(500, "N�o foi poss�vel gerar o token solicitado");

            return Ok(new { token });
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createDto)
        {
            var user = new IdentityUser
            {
                UserName = createDto.Nome.Replace(" ", "QQQ"),
                NormalizedUserName = createDto.Email.ToUpper(),
                Email = createDto.Email,
                PhoneNumber = createDto.Celular,
            };

            var emailJaCadastrado = await _userManager.FindByEmailAsync(createDto.Email);

            if (emailJaCadastrado is not null)
                return StatusCode(500, $"E-mail informado já cadastrado");

            var result = await _userManager.CreateAsync(user, createDto.Senha);

            if (result.Succeeded)
                return await Login(new LoginDto
                {
                    Email = createDto.Email,
                    Senha = createDto.Senha
                });
            else
            {
                string erros = string.Empty;
                foreach (var error in result.Errors)
                {
                    erros += error.Description;
                }
                return StatusCode(500, $"Houveram erros ao criar o usu�rio {erros}");
            }
        }

        private string GerarTokenJwt(IdentityUser user)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:MinutosExpiracao"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
