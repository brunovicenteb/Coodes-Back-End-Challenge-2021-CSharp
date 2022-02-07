using System.Threading.Tasks;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Core.Interfaces.InterfaceServices
{
    public interface XIServiceArticle
    {
        Task AddArticle(XArticle pArticle);
        Task UpdateArticle(XArticle pArticle);
    }
}