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
                 .LeftJoin(dbContext.Products, b => b.ProductId, p => p.Id, (basket, product) => new { basket, product })
                 .LeftJoin(dbContext.Categories, bp => bp.product != null ? bp.product.CategoryId : Guid.Empty, c => c.Id, (bp, category) => new
                 {
                     bp.basket,
                     bp.product,
                     category
                 })
                 .Select(s => new BasketDto(
                     s.basket.Id,
                     s.basket.ProductId,
                     s.product != null ? s.product.Name : string.Empty,
                     s.category != null ? s.category.Name : string.Empty 
                 ))
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