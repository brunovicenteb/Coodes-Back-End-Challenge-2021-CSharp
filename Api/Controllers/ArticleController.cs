using System;
using Microsoft.AspNetCore.Mvc;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Web;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Service.Validators;

namespace Coodesh.Back.End.Challenge2021.CSharp.Api.Controllers
{
    [ApiController]
    [Route("articles")]
    public class ArticleController : XControllerBase
    {
        private XIArticleService _Service;

        public ArticleController(XIArticleService pService)
        {
            _Service = pService;
        }

        /// <response code="200">Response</response>
        [HttpGet("count")]
        public IActionResult Count()
        {
            return Execute(() => _Service.Count());
        }

        /// <response code="200">Response</response>
        /// <param name="_limit">Maximum number of results possible (Limited to 50 results).</param>
        /// <param name="_start">Skip a specific number of entries. This feature is especially useful for pagination.</param>
        [HttpGet]
        public IActionResult Articles(int? _limit = null, int? _start = null)
        {
            return Execute(() => _Service.Get<XArticle>(_limit, _start));
        }

        /// <response code="200">Response</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Identifier of Article.</param>
        [HttpGet("{id}")]
        public IActionResult Articles(int id)
        {
            //Func<IActionResult> notFound = () => NotFound($"Article {id} not found.");
            return Execute(() => _Service.GetObjectByID<XArticle>(id));
        }

        /// <response code="200">Response</response>
        /// <response code="400">Bad Request</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "featured": true,
        ///        "title": "some title",
        ///        "url": "some url",
        ///        "imageUrl": "some imageUrl",
        ///        "newsSite": "some newsSite"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public IActionResult Articles([FromBody] XArticle pArticle)
        {
            return Execute(() => _Service.Add<XArticle, XArticle, XArticleValidator>(pArticle));
        }

        /// <summary>Update a Article</summary>
        /// <response code="200">Response</response>
        /// <response code="404">Bad Request</response>
        /// <param name="id">Identifier of Article.</param>
        /// <param name="pArticle">Article to update (From Body).</param>
        [HttpPut("{id}")]
        public IActionResult ArticlesPut(int id, [FromBody] XArticle pArticle)
        {
            return Execute(() => _Service.Update<XArticle, XArticle, XArticleValidator>(id, pArticle));
        }

        /// <summary>Delete a Article</summary>
        /// <response code="200">Response</response>
        /// <response code="404">Bad Request</response>
        /// <param name="id">Identifier of Article.</param>
        [HttpDelete("{id}")]
        public IActionResult ArticlesDelete(int id)
        {
            return Execute(() => _Service.Delete(id));
        }
    }
}