using ManagerTruck.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ManagerTruck.Web.Dtos;

namespace ManagerTruck.Web.Pages.Caminhao
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public CaminhaoViewModel Caminhao { get; set; }
        private readonly CaminhoesServices _caminhoesServices;
        private readonly JsonSerializerOptions options;
        public DeleteModel(CaminhoesServices caminhoesServices)
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

        public async Task<IActionResult> OnPostAsync()
        {
            _caminhoesServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);

            var response = await _caminhoesServices.DeleteCaminhoesAsync(Caminhao.Id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Caminhao/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao apagar o caminhao.");
            return Page();
        }
    }
}