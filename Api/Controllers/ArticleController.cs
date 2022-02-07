using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System;
using Coodesh.Back.End.Challenge2021.CSharp.Application.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Entities.Entities;

namespace Coodesh.Back.End.Challenge2021.CSharp.Api.Controllers
{

    [ApiController]
    [Route("articles")]
    public class ArticleController : ControllerBase
    {
        private XIArticleApp _App;

        public ArticleController(XIArticleApp pArticleApp)
        {
            _App = pArticleApp;
        }

        /// <response code="200">Response</response>
        [HttpGet("count")]
        public async Task<long> Count()
        {
            return await _App.Count();
        }

        /// <response code="200">Response</response>
        /// <param name="_limit">Maximum number of results possible (Limited to 50 results).</param>
        /// <param name="_start">Skip a specific number of entries. This feature is especially useful for pagination.</param>
        [HttpGet]
        public async Task<IEnumerable<XArticle>> Articles(int? _limit = null, int? _start = null)
        {
            return await _App.Get(_limit, _start);
        }

        /// <response code="200">Response</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Identifier of Article.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Articles(int id)
        {
            XArticle t = await LoadArticle(id);
            if (t != null)
                return Ok(t);
            return NotFound($"Article {id} not found.");
        }

        private async Task<XArticle> LoadArticle(int pArticleID)
        {
            return await _App.GetObjectByID(pArticleID);
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
        public async Task<IActionResult> Articles([FromBody] XArticle pArticle)
        {
            if (pArticle == null)
                return BadRequest("Invalid Article.");
            XArticle a = await _App.Add(pArticle);
            return Ok(a);
        }

        /// <summary>Update a Article</summary>
        /// <response code="200">Response</response>
        /// <response code="404">Bad Request</response>
        /// <param name="id">Identifier of Article.</param>
        /// <param name="pArticle">Article to update (From Body).</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> ArticlesPut(int id, [FromBody] XArticle pArticle)
        {
            if (pArticle == null)
                return BadRequest("Invalid Article.");
            pArticle.ID = id;
            pArticle = await _App.Update(pArticle);
            if (pArticle != null)
                return Ok(pArticle);
            return BadRequest($"Article {id} not updated.");
        }

        /// <summary>Delete a Article</summary>
        /// <response code="200">Response</response>
        /// <response code="404">Bad Request</response>
        /// <param name="id">Identifier of Article.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ArticlesDelete(int id)
        {
            bool sucess = await _App.Delete(id);
            if (sucess)
                return Ok(true);
            return BadRequest($"Article {id} not deleted.");
        }
    }
}