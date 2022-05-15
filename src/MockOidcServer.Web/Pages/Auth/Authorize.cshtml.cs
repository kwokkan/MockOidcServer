using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MockOidcServer.Web.Pages.Auth
{
    public class AuthorizeModel : PageModel
    {
        [BindProperty(Name = "response_type", SupportsGet = true)]
        public string ResponseType { get; set; }

        [BindProperty(Name = "client_id", SupportsGet = true)]
        public string ClientId { get; set; }

        [BindProperty(Name = "scope", SupportsGet = true)]
        public string Scope { get; set; }

        [BindProperty(Name = "redirect_uri", SupportsGet = true)]
        public string RedirectUri { get; set; }

        [BindProperty(Name = "state", SupportsGet = true)]
        public string? State { get; set; }

        [BindProperty(Name = "nonce", SupportsGet = true)]
        public string Nonce { get; set; }

        public void OnGet()
        {
        }
    }
}
