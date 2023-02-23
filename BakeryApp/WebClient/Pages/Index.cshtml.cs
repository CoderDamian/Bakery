using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebClient.Data;
using WebClient.Models;

namespace WebClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MyDbContext _dbContext;

        [BindProperty]
        public string MyCookie { get; set; } = string.Empty;

        [BindProperty]
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        [BindProperty]
        public Product FeaturedProduct { get; set; }

        public IndexModel(ILogger<IndexModel> logger, MyDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public async Task OnGet()
        {
            Products = await _dbContext.Products.ToListAsync().ConfigureAwait(false);

            FeaturedProduct = Products.ElementAt(new Random().Next(Products.Count()));

            if (Request.Cookies.TryGetValue("MyCookie", out var message))
                MyCookie = message;
        }
    }
}