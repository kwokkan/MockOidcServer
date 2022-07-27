using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using MockOidcServer.Web.Models;

namespace MockOidcServer.Web.Pages.Auth
{
    public class AuthorizeModel : PageModel
    {
        public IEnumerable<SelectListItem> ErrorSelectListItems { get; set; } = new List<SelectListItem>
        {
            new SelectListItem("access_denied", "access_denied", true),
            new SelectListItem("invalid_request", "invalid_request"),
            new SelectListItem("invalid_scope", "invalid_scope"),
            new SelectListItem("server_error", "server_error"),
            new SelectListItem("temporarily_unavailable", "temporarily_unavailable"),
            new SelectListItem("unauthorized_client", "unauthorized_client"),
            new SelectListItem("unsupported_response_type", "unsupported_response_type"),
        };

        [BindProperty(Name = "response_type", SupportsGet = true)]
        [DisplayName("response_type")]
        public string ResponseType { get; set; } = default!;

        [BindProperty(Name = "client_id", SupportsGet = true)]
        [DisplayName("client_id")]
        public string ClientId { get; set; } = default!;

        [BindProperty(Name = "scope", SupportsGet = true)]
        [DisplayName("scope")]
        public string Scope { get; set; } = default!;

        [BindProperty(Name = "redirect_uri", SupportsGet = true)]
        [DisplayName("redirect_uri")]
        public string RedirectUri { get; set; } = default!;

        [BindProperty(Name = "state", SupportsGet = true)]
        [DisplayName("state")]
        public string? State { get; set; }

        [BindProperty(Name = "nonce", SupportsGet = true)]
        [DisplayName("nonce")]
        public string Nonce { get; set; } = default!;

        [BindProperty(Name = "username", SupportsGet = true)]
        public string Username { get; set; } = default!;

        [BindProperty(Name = "password", SupportsGet = true)]
        public string? Password { get; set; }

        [BindProperty(Name = "signing_key", SupportsGet = true)]
        [DisplayName("signing_key")]
        public string? SigningKey { get; set; }

        [BindProperty(Name = "error", SupportsGet = true)]
        [DisplayName("error")]
        public string? Error { get; set; }

        [BindProperty(Name = "error_description", SupportsGet = true)]
        [DisplayName("error_description")]
        public string? ErrorDescription { get; set; }

        [BindProperty(Name = "error_uri", SupportsGet = true)]
        [DisplayName("error_uri")]
        public string? ErrorUri { get; set; }

        [BindProperty]
        public string? Action { get; set; }

        public string? Jwt { get; set; }

        public string? ReturnUrl { get; set; }

        public void OnGet()
        {
            Populate();
        }

        public IActionResult OnPost()
        {
            switch (Action)
            {
                case "login":
                    {
                        Populate();

                        return Redirect(ReturnUrl!);

                    }
                case "deny":
                    {
                        PopulateForCancel();

                        return Redirect(ReturnUrl!);
                    }
                default:
                    return Page();
            }
        }

        private void Populate()
        {
            var sub = GenerateSub();
            var jwt = GenerateJwt(sub);

            Jwt = jwt;
            var returnParams = new Dictionary<string, string?>
            {
                { "access_token", sub },
                { "token_type", "Bearer" },
                { "id_token", jwt },
                { "state", State },
                //{ "expires_in", sub },
            };

            var encodedParams = string.Join("&", returnParams.Select(x => x.Key + "=" + HttpUtility.UrlEncode(x.Value)));

            ReturnUrl = $"{RedirectUri}#{encodedParams}";
        }

        private void PopulateForCancel()
        {
            var returnParams = new Dictionary<string, string?>
            {
                { "error", Error },
                { "error_description", ErrorDescription },
                { "error_uri", ErrorUri },
                { "state", State },
            };

            var encodedParams = string.Join("&", returnParams.Select(x => x.Key + "=" + HttpUtility.UrlEncode(x.Value)));

            ReturnUrl = $"{RedirectUri}#{encodedParams}";
        }

        private string GenerateJwt(string sub)
        {
            var myIssuer = "http://mysite.com";
            var myAudience = "http://myaudience.com";
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("sub", sub),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = myIssuer,
                Audience = myAudience,
            };

            if (SigningKey != null)
            {
                //var securityKey = new SymmetricSecurityKey(SigningKey.ToByteArray());
                //tokenDescriptor.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateSub()
        {
            var subJson = new SubModel
            {
                Username = Username,
                Password = Password,
            }.ToJson();

            return Convert.ToBase64String(subJson.ToByteArray()!);
        }
    }
}
