using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ManagerTruck.Web.Dtos;
using System;

public class LoginModel : PageModel
{
    private readonly AutenticacaoServices _autenticacaoServices; 

    public LoginModel(AutenticacaoServices autenticacaoServices)
    {
        _autenticacaoServices = autenticacaoServices;
    }

    [BindProperty]
    public LoginViewModel Login { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var response = await _autenticacaoServices.LoginAsync(Login);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();

            var dtoToken = JsonConvert.DeserializeObject<TokenDto>(token);

            Response.Cookies.Append("jwtToken", dtoToken.token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });

            HttpContext.Session.SetString("jwtToken", dtoToken.token);

            return RedirectToPage("/Home");
        }

        ModelState.AddModelError(string.Empty, "Usuario ou senha incorretos.");
        return Page();

    }
}
