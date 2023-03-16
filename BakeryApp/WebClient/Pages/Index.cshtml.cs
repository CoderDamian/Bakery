using DataTransferObjects.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _options;

        [BindProperty]
        public ListProductsDTO? Product { get; set; }

        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient)
        {
            _logger = logger;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7152/api/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);

            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGet()
        {
            Request.Cookies.TryGetValue("AccessTokenValue", out string? accessValueToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessValueToken);

            var response = await _httpClient.GetAsync("product").ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToPage("./User/Login");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                Product = await JsonSerializer.DeserializeAsync<ListProductsDTO>(content, _options).ConfigureAwait(false);
            }

            return Page();
        }
    }
}