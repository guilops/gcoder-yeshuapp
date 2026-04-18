using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LogoutModel : PageModel
{
    public LogoutModel()
    {
        
    }
    public IActionResult OnGet()
    {
        Response.Cookies.Delete("jwtToken");

        return RedirectToPage("/Login");
    }
}
