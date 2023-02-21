using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebClient.Data;
using WebClient.Models;

namespace WebClient.Pages
{
    public class orderModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        [BindProperty]
        public Product? Product { get; set; }

        public orderModel(MyDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task OnGet(int id)
        {
            Product = await _dbContext.Products.FindAsync(id).ConfigureAwait(false);

            if (Product == null)
                Product = new Product();
        }
    }
}
