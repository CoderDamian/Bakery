﻿using AppliationService;
using AppliationService.Contracts;
using AppliationService.Mappings;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace RESTFul.Extensions
{
    public static class ServicesExtension
    {
        public static void AddBakeryService(this IServiceCollection services)
            => services.AddScoped<IProductService, ProductService>();

        public static void AddAutoMapper(this IServiceCollection services)
            => services.AddAutoMapper(typeof(ProductProfile));

        public static void AddSqlite(this IServiceCollection services)
            => services.AddDbContext<BakeryDbContext>(opt => opt.UseSqlite(@"Data source = bakery.db"));
    }
}