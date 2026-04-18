using ManagerTruck.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ManagerTruck.Web.Pages.Caminhao
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CaminhaoViewModel Caminhao { get; set; } = new CaminhaoViewModel();
        [BindProperty]
        public List<MontadoraViewModel> Montadoras { get; set; }
        private readonly CaminhoesServices _caminhaoServices;
        private readonly MontadorasServices _montadorasServices;
        private readonly JsonSerializerOptions options;

        public CreateModel(CaminhoesServices caminhaoServices,
                           MontadorasServices montadorasServices)
        {
            _caminhaoServices = caminhaoServices;
            _montadorasServices = montadorasServices;
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            _montadorasServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _montadorasServices.GetMontadorasAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception("Não foi possivel carregar as montadoras");

            var json = await response.Content.ReadAsStringAsync();
            Montadoras = JsonSerializer.Deserialize<List<MontadoraViewModel>>(json, options);
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(IFormFile FotoFile)
        {
            if (FotoFile != null)
            {
                using var ms = new MemoryStream();
                await FotoFile.CopyToAsync(ms);
                var bytes = ms.ToArray();
                Caminhao.FotoUrl = $"data:{FotoFile.ContentType};base64,{Convert.ToBase64String(bytes)}";
            }

            _caminhaoServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);

            var response = await _caminhaoServices.CreateCaminhoesAsync(Caminhao);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Caminhao/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar o caminhao.");
            return Page();
        }
    }
}