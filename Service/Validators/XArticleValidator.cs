using FluentValidation;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Service.Validators
{
    public class XArticleValidator : AbstractValidator<XArticle>
    {
        public XArticleValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Título não informado.")
                .NotNull().WithMessage("Título não informado.");

            RuleFor(c => c.Url)
                .NotEmpty().WithMessage("Url não informada.")
                .NotNull().WithMessage("Url não informada.");

            RuleFor(c => c.ImageUrl)
                .NotEmpty().WithMessage("ImageUrl não informada.")
                .NotNull().WithMessage("ImageUrl não informada.");
        }
    }
}