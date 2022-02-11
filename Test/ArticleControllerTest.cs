using System.Linq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Test.Mock;
using Coodesh.Back.End.Challenge2021.CSharp.Api.Controllers;
using Coodesh.Back.End.Challenge2021.CSharp.Service.Services;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using AutoMapper;

namespace Coodesh.Back.End.Challenge2021.CSharp.Test
{
    public class ArticleControllerTest
    {
        [Test]
        public void TestEndpointCount()
        {
            ArticleController c = CreateController(true);
            IActionResult result = c.Count();
            long longResult = AssertOk<long>(result, 15);
            Assert.AreEqual(longResult, 15);
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
            bool removeResult = AssertOk<bool>(result, true);
            Assert.IsTrue(removeResult);

            result = c.Count();
            AssertOk<long>(result, longResult - 1);
        }

        [Test]
        public void TestEndpointArticlesDeleteFalse()
        {
            ArticleController c = CreateController();
            IActionResult result = c.ArticlesDelete(157849);
            bool removeResult = AssertOk<bool>(result, false);
            Assert.IsFalse(removeResult);
        }

        private long GetCount(ArticleController pController)
        {
            IActionResult result = pController.Count();
            OkObjectResult okResult = (OkObjectResult)result;
            return (long)okResult.Value;
        }

        private ArticleController CreateController(bool pLoadData = false)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<XArticle, XArticle>();
            });
            Mapper m = new Mapper(configuration);
            XIArticleService app = new XArticleService(new XMockArticle(pLoadData), m);
            ArticleController c = new ArticleController(app);
            return new ArticleController(app);
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

        private T AssertOk<T>(IActionResult pResult, object pValue)
        {
            Assert.IsInstanceOf<OkObjectResult>(pResult);
            OkObjectResult okResult = (OkObjectResult)pResult;
            Assert.IsInstanceOf<T>(okResult.Value);
            return (T)okResult.Value;
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

        private XArticle CreateArticle(int? pID)
        {
            ArticleController c = CreateController();
            long count = GetCount(c);

            XArticle a = new XArticle();
            if (pID.HasValue)
                a.ID = pID.Value;
            a.Featured = true;
            a.Title = "James Webb reach lagrange point 2";
            a.Summary = "On 24 January, 30 days after launch on Christmas Day, the James Webb Space Telescope...";
            a.Url = "This is a Url content";
            a.ImageUrl = "This is a ImageUrl content";

            IActionResult result = c.Articles(a);
            XArticle newArticle = AssertOk<XArticle>(result, a);
            Assert.IsTrue(newArticle.Featured);
            Assert.IsTrue(string.IsNullOrEmpty(a.ObjectID));
            Assert.AreNotSame(a, newArticle);
            Assert.AreEqual("James Webb reach lagrange point 2", newArticle.Title);
            Assert.AreEqual("On 24 January, 30 days after launch on Christmas Day, the James Webb Space Telescope...", newArticle.Summary);
            Assert.AreEqual("This is a Url content", newArticle.Url);
            Assert.AreEqual("This is a ImageUrl content", newArticle.ImageUrl);
            long newCount = GetCount(c);
            Assert.AreEqual(count + 1, newCount);
            return newArticle;
        }
    }
}