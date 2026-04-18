using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;

namespace ManagerTruck.Web.Pages
{
    public class HomeModel : PageModel
    {

        public string Saudacao { get; set; } = string.Empty;

        public HomeModel()
        {
        }

        public async Task<IActionResult> OnGet()
        {
            var jwtToken = Request.Cookies["jwtToken"] ?? throw new UnauthorizedAccessException("Token JWT nao encontrado.");
            
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var username = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            username = username.Replace("QQQ", " ");

            var fusoHorario = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            var horaBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fusoHorario);
            var hora = horaBrasilia.Hour;

            if (hora >= 5 && hora < 12)
                Saudacao = "Bom dia";
            else if (hora >= 12 && hora < 18)
                Saudacao = "Boa tarde";
            else
                Saudacao = "Boa noite";

            if (!string.IsNullOrEmpty(username))
                Saudacao += $", {username}";

            return Page();
        }
    }
}
