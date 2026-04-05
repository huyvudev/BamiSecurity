using Microsoft.AspNetCore.Mvc;

namespace CR.IdentityServerBase.Dto
{
    public class ConnectTokenDto
    {
        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; } = null!;

        [FromForm(Name = "code")]
        public string? Code { get; set; }

        [FromForm(Name = "code_verifier")]
        public string? CodeVerifier { get; set; }

        [FromForm(Name = "username")]
        public string? Username { get; set; }

        [FromForm(Name = "password")]
        public string? Password { get; set; }

        [FromForm(Name = "scope")]
        public string? Scope { get; set; }

        [FromForm(Name = "client_id")]
        public string? ClientId { get; set; }

        [FromForm(Name = "client_secret")]
        public string? ClientSecret { get; set; }

        [FromForm(Name = "fcm_token")]
        public string? FcmToken { get; set; }

        [FromForm(Name = "apns_token")]
        public string? ApnsToken { get; set; }
    }
}
