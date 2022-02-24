using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
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


        /// <summary>Returns the total number of registered articles.</summary>
        [HttpGet("count")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Count()
        {
            return TryExecuteOK(() => _Service.Count());
        }


        /// <summary>Returns the registered articles with the possibility of pagination.</summary>
        /// <param name="_limit">Maximum number of results possible (Limited to 50 results).</param>
        /// <param name="_start">Skip a specific number of entries. This feature is especially useful for pagination.</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<XArticle>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Articles(int? _limit = null, int? _start = null)
        {
            return TryExecuteOK(() => _Service.Get(_limit, _start));
        }

        /// <summary>Returns an article by identifier.</summary>
        /// <param name="id">Identifier of article.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(XArticle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Articles(int id)
        {
            return TryExecuteOK(() => _Service.GetObjectByID(id));
        }


        /// <summary>Create a new article.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(XArticle), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Articles([FromBody] XArticle pArticle)
        {
            Func<object> execute = delegate
            {
                return _Service.Add<XArticleValidator>(pArticle);
            };
            Func<object, IActionResult> action = delegate (object result)
            {
                XArticle a = result as XArticle;
                return CreatedAtAction(nameof(Articles).ToLower(), new { id = a.ID }, result);
            };
            return TryExecute(action, execute);
        }

        /// <summary>Update a article.</summary>
        /// <param name="id">Identifier of article.</param>
        /// <param name="pArticle">Article to update.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(XArticle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ArticlesPut(int id, [FromBody] XArticle pArticle)
        {
            return TryExecuteOK(() => _Service.Update<XArticleValidator>(id, pArticle));
        }

        /// <summary>Delete a article.</summary>
        /// <param name="id">Identifier of article.</param>
        /// <remarks>When you delete an article, it will be permanently removed from the base.</remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ArticlesDelete(int id)
        {
            return TryExecuteDelete(() => _Service.Delete(id));
        }
    }
}