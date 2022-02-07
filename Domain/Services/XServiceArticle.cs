using System.Threading.Tasks;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Interfaces.InterfaceArticle;
using Coodesh.Back.End.Challenge2021.CSharp.Core.Interfaces.InterfaceServices;

namespace Coodesh.Back.End.Challenge2021.CSharp.Core.Services
{
    public class XServiceArticle : XIServiceArticle
    {
        private readonly XIArticle _Article;

        public XServiceArticle(XIArticle pArticle)
        {
            _Article = pArticle;
        }

        public async Task AddArticle(XArticle pArticle)
        {
            //var checkName = pArticle.CheckString(pArticle.Name, "Nome");
            //var checkPrice = pArticle.CheckDecimal(pArticle.Price, "Preco");
            //if (checkName && checkPrice)
            //{
            //pArticle.IsActive = true;
            await _Article.Add(pArticle);
            //}
        }

        public async Task UpdateArticle(XArticle pArticle)
        {
            //var checkName = pProduct.CheckString(pProduct.Name, "Nome");
            //var checkPrice = pProduct.CheckDecimal(pProduct.Price, "Preco");
            //if (checkName && checkPrice)
            await _Article.Update(pArticle);
        }
    }
}