using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coodes.Back.End.Challenge2021.CSharp.Core.Data;
using Coodes.Back.End.Challenge2021.CSharp.Core.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System;

namespace CoodesBackEndChallenge2021CSharp.Controllers
{

    [ApiController]
    [Route("articles")]
    public class ArticleController : ControllerBase
    {
        private IArticleContext _Context;

        public ArticleController(IArticleContext pContext)
        {
            _Context = pContext;
        }

        /// <response code="200">Response</response>
        [HttpGet("count")]
        public async Task<long> Count()
        {
            return await _Context.Articles.CountDocumentsAsync(FilterDefinition<Article>.Empty);
        }

        /// <response code="200">Response</response>
        /// <param name="_limit">Maximum number of results possible (Limited to 50 results).</param>
        /// <param name="_start">Skip a specific number of entries. This feature is especially useful for pagination.</param>
        [HttpGet]
        public async Task<IEnumerable<Article>> Articles(int? _limit = null, int? _start = null)
        {
            int skip = _start ?? 0;
            int limit = Math.Min(50, _limit ?? 10);
            return await _Context.Articles.Find(FilterDefinition<Article>.Empty)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }

        /// <response code="200">Response</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Identifier of Article.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Articles(int id)
        {
            Article t = await LoadArticle(id);
            if (t != null)
                return Ok(t);
            return NotFound($"Article {id} not found.");
        }

        private Task<Article> LoadArticle(int pArticleID)
        {
            var builder = Builders<Article>.Filter;
            var filter = builder.Eq("ID", pArticleID);
            return _Context.Articles.Find(filter).FirstOrDefaultAsync();
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
        public async Task<IActionResult> Articles([FromBody] Article pArticle)
        {
            if (pArticle == null)
                return BadRequest("Invalid Article.");
            int newID = pArticle.ID;
            if (newID <= 0)
                newID = pArticle.ID = Guid.NewGuid().GetHashCode();
            await _Context.Articles.InsertOneAsync(pArticle);
            return await Articles(newID);
        }

        /// <summary>Update a Article</summary>
        /// <response code="200">Response</response>
        /// <response code="404">Bad Request</response>
        /// <param name="id">Identifier of Article.</param>
        /// <param name="pArticle">Article to update (From Body).</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> ArticlesPut(int id, [FromBody] Article pArticle)
        {
            if (pArticle == null)
                return BadRequest("Invalid Article.");
            Article t = await LoadArticle(id);
            if (t == null)
                return NotFound($"Article {id} not found.");
            pArticle.ID = id;
            pArticle.ObjectID = t.ObjectID;
            var updateResult = await _Context.Articles.ReplaceOneAsync(
                o => o.ObjectID == t.ObjectID, replacement: pArticle);
            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
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
            Article t = await LoadArticle(id);
            if (t == null)
                return NotFound($"Article {id} not found.");
            var deleteResult = await _Context.Articles.DeleteOneAsync(
                o => o.ID == id);
            if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
                return Ok(true);
            return BadRequest($"Article {id} not deleted.");
        }
    }
}