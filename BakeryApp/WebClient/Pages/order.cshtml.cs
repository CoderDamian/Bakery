using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using WebClient.Data;
using WebClient.Models;

namespace WebClient.Pages
{
    public class orderModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        [BindProperty]
        public Product? Product { get; set; }

        [BindProperty, EmailAddress, Required, Display(Name = "Your email address")]
        public string OrderEmail { get; set; }

        [BindProperty, Required(ErrorMessage = "Please supply a shipping address"), Display(Name = "Shipping address")]
        public string OrderShipping { get; set; }

        [BindProperty, Display(Name = "Quantity")]
        public int OrderQuantity { get; set; }

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

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
                return RedirectToPage("OrderSuccess");

            return Page();
        }
    }
}
