using Carter;
using Microsoft.EntityFrameworkCore;
using WebAPIEgitimi.MinimalAPI.Context;
using WebAPIEgitimi.MinimalAPI.Dto;
using WebAPIEgitimi.MinimalAPI.Models;

namespace WebAPIEgitimi.MinimalAPI.Modules
{
    public sealed class BasketModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder group)
        {
            var app = group.MapGroup("baskets").WithTags("Baskets");

            app.MapGet(string.Empty, (ApplicationDbContext dbContext) => {

                var res = dbContext.Baskets
                    .LeftJoin(dbContext.Products, b => b.ProductId, p=>p.Id, (basket, product) => new { basket, product })
                    .Select(s => new BasketDto(
                        s.basket.Id, 
                        s.product.Id, 
                        s.product != null ? s.product.Name : string.Empty))
                    .ToList();

                return res;
            });

            app.MapPost(string.Empty, (BasketCreateDto request, ApplicationDbContext dbContext) =>
            {
                Basket basket = new()
                {
                    ProductId = request.ProductId
                };
                dbContext.Baskets.Add(basket);
                dbContext.SaveChanges();
                return new { Message = "Product successfully added to the basket!" };
            });
    }
    }
}