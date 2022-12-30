using ApiProdutos.Data;
using ApiProdutos.Filters;
using ApiProdutos.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ApiProdutos.Endpoints
{
    public static class ProdutosEndpoints
    {
        public static void MapProdutosEndpoints(this WebApplication app)
        {
            app.MapGet("/produtos", List);
            app.MapGet("/produtos/{id}", Get);
            app.MapPost("/produtos", Create).AddEndpointFilter<ValidationFilter<Produto>>();
            app.MapPut("/produtos", Update).AddEndpointFilter<ValidationFilter<Produto>>();
            app.MapDelete("/produtos/{id}", Delete);
        }
        public static async Task<IResult> List(AppDbContext db)
        {
            var result = await db.Produtos.ToListAsync();
            return Results.Ok(result);
        }
        public static async Task<IResult> Get(AppDbContext db , int id)
        {
            return await db.Produtos.FindAsync(id) is Produto produto
                ? Results.Ok(produto)
                : Results.NotFound();
        }
        public static async Task<IResult> Create(AppDbContext db,
                        IValidator<Produto> validator, Produto produto)
        {
            db.Produtos.Add(produto);
            await db.SaveChangesAsync();

            return Results.Created($"/produtos/{produto.Id}", produto);
        }
        public static async Task<IResult> Update(AppDbContext db,
                        IValidator<Produto> validator, Produto produtoAtualizado)
        {
            var produto = await db.Produtos.FindAsync(produtoAtualizado.Id);
            if (produto is null) return Results.NotFound();

            produto.Nome = produtoAtualizado.Nome;
            produto.Preco = produtoAtualizado.Preco;
            produto.Estoque = produtoAtualizado.Estoque;

            await db.SaveChangesAsync();
            return Results.NoContent();
        }
        public static async Task<IResult> Delete(AppDbContext db, IValidator<Produto> validator, int id)
        {
            if (await db.Produtos.FindAsync(id) is Produto produto)
            {
                db.Produtos.Remove(produto);
                await db.SaveChangesAsync();
                return Results.Ok(produto);
            }

            return Results.NotFound();
        }

       
    }
}
