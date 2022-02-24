using System;
using AutoMapper;
using System.Linq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Coodesh.Back.End.Challenge2021.CSharp.Test.Mock;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Api.Controllers;
using Coodesh.Back.End.Challenge2021.CSharp.Service.Caching;
using Coodesh.Back.End.Challenge2021.CSharp.Service.Services;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Interfaces;

namespace Coodesh.Back.End.Challenge2021.CSharp.Test
{
    public class ArticleControllerTest
    {
        [Test]
        public void TestEndpointCount()
        {
            XMockCache cache = new XMockCache();
            ArticleController c = CreateController(true, cache);
            IActionResult result = c.Count();
            long longResult = AssertOk<long>(result, 15);
            Assert.AreEqual(longResult, 15);
            Assert.IsFalse(cache.IsLastHit);

            result = c.Count(); // Call again to test cache.
            longResult = AssertOk<long>(result, 15);
            Assert.AreEqual(longResult, 15);
            Assert.IsTrue(cache.IsLastHit);
        }

        [Test]
        public void TestEndpointCreateArticleWithoutID()
        {
            XArticle a = CreateArticle(null);
            Assert.IsTrue(a.ID < 0);
        }

        [Test]
        public void TestEndpointCreateArticleWithID()
        {
            XArticle a = CreateArticle(15788745);
            Assert.AreEqual(15788745, a.ID);
        }

        [Test]
        public void TestEndpointCreateArticleWithErrors()
        {
            ArticleController c = CreateController();
            IActionResult result = c.Articles(null);
            AssertError(result, "Invalid Article.");

            XArticle a = CreateArticleObject(5789654, null, null, null);
            result = c.Articles(a);
            AssertError(result, $"Validation failed: {Environment.NewLine} -- Title: Title not informed.{Environment.NewLine} -- Url: Url not informed.{Environment.NewLine} -- ImageUrl: ImageUrl not informed.");

            a.Title = "Starlink Mission";
            result = c.Articles(a);
            AssertError(result, $"Validation failed: {Environment.NewLine} -- Url: Url not informed.{Environment.NewLine} -- ImageUrl: ImageUrl not informed.");

            a.Url = "This is a Url content";
            result = c.Articles(a);
            AssertError(result, $"Validation failed: {Environment.NewLine} -- ImageUrl: ImageUrl not informed.");
        }

        [Test]
        public void TestEndpointUpdateArticle()
        {
            ArticleController c = CreateController(true);
            IActionResult result = c.Count();
            long longResult = AssertOk<long>(result, 15);

            result = c.Articles(4067); // Starlink Mission
            Assert.IsInstanceOf<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<XArticle>(okResult.Value);
            XArticle a = (XArticle)okResult.Value;
            a.Title = a.Title += " [Updated]";
            c.ArticlesPut(4067, a);
            AssertStarlinkMission(a, "Starlink Mission [Updated]");

            result = c.Count();
            AssertOk<long>(result, longResult);
        }

        [Test]
        public void TestEndpointUpdateArticleErrors()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<XArticle, XArticle>();
            });
            Mapper m = new Mapper(configuration);
            ArticleController c = CreateController(true);
            IActionResult result = c.ArticlesPut(4067, null);
            AssertError(result, "Invalid Article.");

            result = c.Articles(4067); // Starlink Mission
            Assert.IsInstanceOf<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<XArticle>(okResult.Value);
            XArticle a = m.Map<XArticle>(okResult.Value);

            result = c.ArticlesPut(154787985, a);
            AssertNotFoundError(result, "Article 154787985 not found.");

            a.Title = a.Url = a.ImageUrl = string.Empty;
            result = c.ArticlesPut(4067, a);
            AssertError(result, $"Validation failed: {Environment.NewLine} -- Title: Title not informed.{Environment.NewLine} -- Url: Url not informed.{Environment.NewLine} -- ImageUrl: ImageUrl not informed.");

            a.Title = "Starlink Mission";
            result = c.ArticlesPut(4067, a);
            AssertError(result, $"Validation failed: {Environment.NewLine} -- Url: Url not informed.{Environment.NewLine} -- ImageUrl: ImageUrl not informed.");

            a.Url = "This is a Url content";
            result = c.ArticlesPut(4067, a);
            AssertError(result, $"Validation failed: {Environment.NewLine} -- ImageUrl: ImageUrl not informed.");
        }

        [Test]
        public void TestEndpointGetArticleByID()
        {
            ArticleController c = CreateController(true);
            IActionResult result = c.Articles(4067);
            Assert.IsInstanceOf<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<XArticle>(okResult.Value);
            XArticle a = (XArticle)okResult.Value;
            AssertStarlinkMission(a);
        }

        [Test]
        public void TestEndpointGetArticleByIDError()
        {
            ArticleController c = CreateController(true);
            IActionResult result = c.Articles(989857457);
            AssertNotFoundError(result, "Article 989857457 not found.");
        }

        [Test]
        public void TestEndpointSearchArticleEmpty()
        {
            ArticleController c = CreateController();
            IActionResult result = c.Articles();
            Assert.IsInstanceOf<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<IEnumerable<XArticle>>(okResult.Value);
            IEnumerable<XArticle> enm = (IEnumerable<XArticle>)okResult.Value;
            XArticle[] resultArticles = enm.ToArray();
            Assert.AreEqual(0, resultArticles.Length);
        }

        [Test]
        public void TestEndpointSearchPaginatedArticles()
        {
            // Testa paginação retornando o valor limite padrão de 10 artigos
            ArticleController c = AssertSearchPaginatedArticles(null, null, 10);

            // Testar pular mais do que o total de 5 em 5 artigos
            AssertSearchPaginatedArticles(16, null, 0);

            // Testar paginação de 5 em 5 artigos
            AssertSearchPaginatedArticles(null, 5, 5);
            AssertSearchPaginatedArticles(5, 5, 5);
            AssertSearchPaginatedArticles(10, 5, 5);
            AssertSearchPaginatedArticles(15, 5, 0);

            // Testar paginação de 10 em 10 artigos
            AssertSearchPaginatedArticles(null, 10, 10);
            AssertSearchPaginatedArticles(10, 10, 5);
            AssertSearchPaginatedArticles(15, 5, 0);
        }

        [Test]
        public void TestEndpointArticlesDeleteTrue()
        {
            ArticleController c = CreateController(true);
            IActionResult result = c.Count();
            long longResult = AssertOk<long>(result, 15);

            result = c.ArticlesDelete(4067);
            Assert.IsInstanceOf<NoContentResult>(result);

            result = c.Count();
            AssertOk<long>(result, longResult - 1);
        }

        [Test]
        public void TestEndpointArticlesDeleteFalse()
        {
            ArticleController c = CreateController(true);
            IActionResult result = c.Count();
            long longResult = AssertOk<long>(result, 15);

            result = c.ArticlesDelete(157849);
            Assert.IsInstanceOf<NotFoundResult>(result);

            result = c.Count();
            AssertOk<long>(result, 15);
        }

        private long GetCount(ArticleController pController)
        {
            IActionResult result = pController.Count();
            OkObjectResult okResult = (OkObjectResult)result;
            return (long)okResult.Value;
        }

        private ArticleController CreateController(bool pLoadData = false, XICache pCache = null)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<XArticleServiceDecorator>();
            XIArticleService app = new XArticleService(new XMockArticle(pLoadData));
            XIArticleService appCache = new XArticleServiceDecorator(app, pCache ?? new XMockCache(), logger);
            return new ArticleController(appCache);
        }

        public ArticleController AssertSearchPaginatedArticles(int? pStart, int? pLimit, int pAmount, ArticleController pController = null)
        {
            ArticleController c = pController ?? CreateController(true); // Alimenta 15 artigos.
            IActionResult result = c.Articles(pLimit, pStart);

            Assert.IsInstanceOf<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<IEnumerable<XArticle>>(okResult.Value);
            IEnumerable<XArticle> enm = (IEnumerable<XArticle>)okResult.Value;
            XArticle[] resultArticles = enm.ToArray();
            Assert.AreEqual(pAmount, resultArticles.Length);
            return c;
        }

        private void AssertNotFoundError(IActionResult pResult, string pMessage)
        {
            Assert.IsInstanceOf<NotFoundObjectResult>(pResult);
            NotFoundObjectResult errorResult = (NotFoundObjectResult)pResult;
            Assert.IsInstanceOf<string>(errorResult.Value);
            Assert.AreEqual(pMessage, errorResult.Value);
        }

        private void AssertError(IActionResult pResult, string pMessage)
        {
            Assert.IsInstanceOf<BadRequestObjectResult>(pResult);
            BadRequestObjectResult errorResult = (BadRequestObjectResult)pResult;
            Assert.IsInstanceOf<string>(errorResult.Value);
            Assert.AreEqual(pMessage, errorResult.Value);
        }

        private T AssertOk<T>(IActionResult pResult, object pValue)
        {
            Assert.IsInstanceOf<OkObjectResult>(pResult);
            OkObjectResult okResult = (OkObjectResult)pResult;
            Assert.IsInstanceOf<T>(okResult.Value);
            return (T)okResult.Value;
        }

        private T AssertOkCreated<T>(IActionResult pResult, object pValue)
        {
            Assert.IsInstanceOf<CreatedAtActionResult>(pResult);
            CreatedAtActionResult createdResult = (CreatedAtActionResult)pResult;
            Assert.IsInstanceOf<T>(createdResult.Value);
            return (T)createdResult.Value;
        }

        private void AssertStarlinkMission(XArticle pArticle, string pTitle = "Starlink Mission")
        {
            Assert.AreEqual(4067, pArticle.ID);
            Assert.AreEqual(false, pArticle.Featured);
            Assert.AreEqual(pTitle, pArticle.Title);
            Assert.AreEqual("https://www.spacex.com/news/2020/01/07/starlink-mission", pArticle.Url);
            Assert.AreEqual("https://www.spacex.com/sites/spacex/files/styles/featured_news_widget_image/public/field/image/starlink_2_outtower_website.jpg?itok=-i8nhHqy", pArticle.ImageUrl);
            Assert.AreEqual("SpaceX", pArticle.NewsSite);
            Assert.AreEqual(string.Empty, pArticle.Summary);
            Assert.AreEqual("2020-01-07T00:00:00.000Z", pArticle.PublishedAt);
            Assert.AreEqual("2021-05-18T13:45:49.196Z", pArticle.UpdatedAt);
            Assert.AreEqual(0, pArticle.Launches.Length);
            Assert.AreEqual(0, pArticle.Events.Length);
        }

        private XArticle CreateArticleObject(int? pID, string pTitle, string pUrl, string pImageUrl)
        {
            XArticle a = new XArticle();
            if (pID.HasValue)
                a.ID = pID.Value;
            a.Featured = false;
            a.Title = pTitle;
            a.Summary = "On 24 January, 30 days after launch on Christmas Day, the James Webb Space Telescope...";
            a.Url = pUrl;
            a.ImageUrl = pImageUrl;
            return a;
        }

        private XArticle CreateArticle(int? pID, string pTitle = "James Webb reach lagrange point 2", string pUrl = "This is a Url content", string pImageUrl = "This is a ImageUrl content")
        {
            ArticleController c = CreateController();
            long count = GetCount(c);
                
            XArticle a = CreateArticleObject(pID, pTitle, pUrl, pImageUrl);

            IActionResult result = c.Articles(a);
            XArticle newArticle = AssertOkCreated<XArticle>(result, a);
            CreatedAtActionResult resultCreated = (CreatedAtActionResult)result;
            Assert.AreEqual("articles", resultCreated.ActionName);
            Assert.AreEqual(1, resultCreated.RouteValues.Count);
            Assert.AreEqual("id", resultCreated.RouteValues.First().Key);
            Assert.AreEqual(a.ID, resultCreated.RouteValues.First().Value);
            Assert.IsFalse(newArticle.Featured);
            Assert.IsTrue(string.IsNullOrEmpty(a.ObjectID));
            Assert.AreSame(a, newArticle);
            Assert.AreEqual(pTitle, newArticle.Title);
            Assert.AreEqual("On 24 January, 30 days after launch on Christmas Day, the James Webb Space Telescope...", newArticle.Summary);
            Assert.AreEqual(pUrl, newArticle.Url);
            Assert.AreEqual(pImageUrl, newArticle.ImageUrl);
            long newCount = GetCount(c);
            Assert.AreEqual(count + 1, newCount);
            return newArticle;
        }
    }
}