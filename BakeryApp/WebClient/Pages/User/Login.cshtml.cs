using DataTransferObjects.DTOs.Token;
using DataTransferObjects.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace WebClient.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        [BindProperty]
        public LoginUserDTO? UserDTO { get; set; }

        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7152/api/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);

            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var userSerialize = JsonSerializer.Serialize(UserDTO);
            var requestContent = new StringContent(userSerialize, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth", requestContent)
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content
                    .ReadAsStreamAsync()
                    .ConfigureAwait(false);

                TokenDTO? tokenDTO = await JsonSerializer.DeserializeAsync<TokenDTO>(content, _options)
                    .ConfigureAwait(false);

                MakeSessionCookie(tokenDTO);

                return RedirectToPage("/Index");
            }

            return Page();
        }

        private void MakeSessionCookie(TokenDTO tokenDTO)
        {
            Response.Cookies.Append("AccessTokenValue", tokenDTO.AccessTokenValue);
        }
    }
}
