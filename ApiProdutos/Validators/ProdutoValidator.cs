using ApiProdutos.Models;
using FluentValidation;

namespace ApiProdutos.Validators
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(o => o.Nome).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(o => o.Preco).NotNull().NotEmpty().NotEqual(0);
            RuleFor(o => o.Estoque).NotNull().NotEmpty().NotEqual(0);
        }
    }
}
