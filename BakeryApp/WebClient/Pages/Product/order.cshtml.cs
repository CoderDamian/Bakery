using DataTransferObjects.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Pages
{
    public class orderModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _options;

        [BindProperty]
        public ProductDTO? Product { get; set; }

        [BindProperty, EmailAddress, Required, Display(Name = "Your email address")]
        public string OrderEmail { get; set; }

        [BindProperty, Required(ErrorMessage = "Please supply a shipping address"), Display(Name = "Shipping address")]
        public string OrderShipping { get; set; }

        [BindProperty, Display(Name = "Quantity")]
        public int OrderQuantity { get; set; }

        public orderModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7152/api/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);

            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task OnGet(int id)
        {
            Request.Cookies.TryGetValue("AccessTokenValue", out string? accessValueToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessValueToken);

            var response = await _httpClient.GetAsync($"product/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                Product = await JsonSerializer.DeserializeAsync<ProductDTO>(content, _options);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
                return RedirectToPage("/product/OrderSuccess");

            return Page();
        }
    }
}
