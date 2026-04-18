using ManagerTruck.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ManagerTruck.Web.Pages.Caminhao
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public CaminhaoViewModel Caminhao { get; set; }
        private readonly CaminhoesServices _caminhoesServices;
        private readonly JsonSerializerOptions options;

        public EditModel(CaminhoesServices caminhoesServices)
        {
            _caminhoesServices = caminhoesServices;
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };            
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            _caminhoesServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _caminhoesServices.GetCaminhoesByIdAsync(id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Caminhao = JsonSerializer.Deserialize<CaminhaoViewModel>(json, options);
                return Page();
            }

            return RedirectToPage("/Caminhao/Index");
        }

        public async Task<IActionResult> OnPostAsync(IFormFile FotoFile)
        {
            if (!ModelState.IsValid)
                return Page();

            if (FotoFile != null)
            {
                using var ms = new MemoryStream();
                await FotoFile.CopyToAsync(ms);
                var bytes = ms.ToArray();
                Caminhao.FotoUrl = $"data:{FotoFile.ContentType};base64,{Convert.ToBase64String(bytes)}";
            }

            _caminhoesServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _caminhoesServices.UpdateCaminhoesAsync(Caminhao);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Caminhao/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao editar essa despesa.");
            return Page();
        }
    }
}