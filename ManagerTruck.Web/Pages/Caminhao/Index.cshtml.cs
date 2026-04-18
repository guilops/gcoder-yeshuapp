using ManagerTruck.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ManagerTruck.Web.Pages.Caminhao
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<CaminhaoViewModel>? Caminhoes { get; set; }
        private readonly CaminhoesServices _caminhoesServices;
        public IndexModel(CaminhoesServices caminhoesServices)
        {
            _caminhoesServices = caminhoesServices;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Login");

            _caminhoesServices.SetAuthorizationHeader(token);
            var result = await _caminhoesServices.GetCaminhoesAsync();

            if (!result.IsSuccessStatusCode)
                return Page();

            var caminhoes = await result.Content.ReadFromJsonAsync<List<CaminhaoViewModel>>();
            Caminhoes = caminhoes;

            return Page();
        }
    }
}