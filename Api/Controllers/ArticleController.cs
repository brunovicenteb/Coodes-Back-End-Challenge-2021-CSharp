using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Web;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Service.Validators;
using System.Collections.Generic;

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

        [HttpGet("count")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public IActionResult Count()
        {
            return TryExecuteOK(() => _Service.Count());
        }

        /// <param name="_limit">Maximum number of results possible (Limited to 50 results).</param>
        /// <param name="_start">Skip a specific number of entries. This feature is especially useful for pagination.</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<XArticle>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public IActionResult Articles(int? _limit = null, int? _start = null)
        {
            return TryExecuteOK(() => _Service.Get(_limit, _start));
        }

        /// <param name="id">Identifier of Article.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(XArticle), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public IActionResult Articles(int id)
        {
            //Func<IActionResult> notFound = () => NotFound($"Article {id} not found.");
            return TryExecuteOK(() => _Service.GetObjectByID(id));
        }

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
        [ProducesResponseType(typeof(XArticle), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Articles([FromBody] XArticle pArticle)
        {
            Func<object> execute = delegate
            {
                return _Service.Add<XArticleValidator>(pArticle);
            };
            Func<object, IActionResult> action = delegate (object result)
            {
                XArticle a = result as XArticle;
                return CreatedAtAction(nameof(Articles), new { id = a.ID }, result);
            };
            return TryExecute(action, execute);
        }

        /// <summary>Update a Article</summary>
        /// <param name="id">Identifier of Article.</param>
        /// <param name="pArticle">Article to update (From Body).</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(XArticle), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult ArticlesPut(int id, [FromBody] XArticle pArticle)
        {
            return TryExecuteOK(() => _Service.Update<XArticleValidator>(id, pArticle));
        }

        /// <summary>Delete a Article</summary>
        /// <param name="id">Identifier of Article.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public IActionResult ArticlesDelete(int id)
        {
            return TryExecuteOK(() => _Service.Delete(id));
        }
    }
}