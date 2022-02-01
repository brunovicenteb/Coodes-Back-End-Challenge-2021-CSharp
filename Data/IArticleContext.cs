using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoodesBackEndChallenge2021CSharp.Entities;

namespace CoodesBackEndChallenge2021CSharp.Data
{
    public interface IArticleContext
    {
        IMongoCollection<Article> Articles
        {
            get;
        }
    }
}