using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Attributes;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Domain.Queries
{
    public class XArticleQuery : XICustomQueryable
    {
        public bool Featured { get; set; }

        [QueryOperator(Operator = WhereOperator.Contains)]
        public string Title { get; set; }

        [QueryOperator(Operator = WhereOperator.Contains)]
        public string Summary { get; set; }

        public int? Limit { get; set; } = 10;

        public int? Offset { get; set; }
    }
}