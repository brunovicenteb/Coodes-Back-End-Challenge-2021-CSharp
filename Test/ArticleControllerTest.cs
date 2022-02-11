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
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEndpointCount()
        {
            ArticleController c = CreateController(true);
            IActionResult result = c.Count();
            long longResult = AssertOk<long>(result, 15);
            Assert.AreEqual(longResult, 15);
        }

        [Test]
        public void TestEndpointArticlesPutWithID()
        {
            XArticle a = TestEndpointArticlesPut(null);
            Assert.IsTrue(a.ID < 0);
        }

        [Test]
        public void TestEndpointArticlesPutWithoutID()
        {
            XArticle a = TestEndpointArticlesPut(15788745);
            Assert.AreEqual(15788745, a.ID);
        }

        [Test]
        public void TestEndpointArticlesEmpty()
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
        public void TestEndpointArticlesRemoveNotExists()
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

        private T AssertOk<T>(IActionResult pResult, object pValue)
        {
            Assert.IsInstanceOf<OkObjectResult>(pResult);
            OkObjectResult okResult = (OkObjectResult)pResult;
            Assert.IsInstanceOf<T>(okResult.Value);
            return (T)okResult.Value;
        }

        private XArticle TestEndpointArticlesPut(int? pID)
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


        //[Test]
        //public void TestEndpointArticlesEmpty()
        //{
        //    var mock = new Mock<XIArticle>(MockBehavior.Loose);
        //    mock.Setup(o => o.Get(null, null)).ReturnsAsync(() => new List<XArticle>());
        //    XIArticleApp app = new XAppArticle(mock.Object);

        //    ArticleController c = new ArticleController(app);
        //    XArticle[] resultArticles = c.Articles().Result.ToArray();
        //    Assert.AreEqual(0, resultArticles.Length);


        //    mock.Setup(o => o.Get(10, null)).ReturnsAsync(() => new List<XArticle>());
        //    resultArticles = c.Articles().Result.ToArray();
        //    Assert.AreEqual(0, resultArticles.Length);

        //    mock.Setup(o => o.Get(10, 10)).ReturnsAsync(() => new List<XArticle>());
        //    resultArticles = c.Articles().Result.ToArray();
        //    Assert.AreEqual(0, resultArticles.Length);
        //}
    }
}