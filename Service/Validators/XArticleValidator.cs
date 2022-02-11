using FluentValidation;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Service.Validators
{
    public class XArticleValidator : AbstractValidator<XArticle>
    {
        public XArticleValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Title not informed.");

            RuleFor(c => c.Url)
                .NotEmpty().WithMessage("Url not informed.");

            RuleFor(c => c.ImageUrl)
                .NotEmpty().WithMessage("ImageUrl not informed.");
        }
    }
}