using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ManagerTruck.Web.Dtos;

public class NovaManutencaoModel : PageModel
{
    private readonly CaminhoesServices _caminhaoServices;

    [BindProperty]
    public ManutencaoInput Input { get; set; }

    public NovaManutencaoModel(CaminhoesServices caminhaoServices)
    {
        _caminhaoServices = caminhaoServices;
    }

    public void OnGetAsync() { }

    public async Task<IActionResult> OnPost()
    {
        _caminhaoServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);

        var response = await _caminhaoServices.CreateManutencaoCaminhoesAsync(Input);

        if (response.IsSuccessStatusCode)
            return RedirectToPage("/Caminhao/Index");

        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a manutenção do caminhao.");
        return Page();
    }
}