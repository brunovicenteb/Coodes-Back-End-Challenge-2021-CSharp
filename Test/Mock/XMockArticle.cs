using System;
using System.Linq;
using System.Collections.Generic;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Entities;
using Coodesh.Back.End.Challenge2021.CSharp.Domain.Interfaces;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace Coodesh.Back.End.Challenge2021.CSharp.Test.Mock
{
    public class XMockArticle : XIArticleRepository
    {
        private readonly List<XArticle> _Articles = new List<XArticle>();

        public XMockArticle(bool pLoadData)
        {
            if (!pLoadData)
                return;
            Assembly assembly = Assembly.GetExecutingAssembly();
            var resourcePath = String.Format("{0}.{1}", Regex.Replace(assembly.ManifestModule.Name, @"\.(exe|dll)$",
              string.Empty, RegexOptions.IgnoreCase), "Resources.ArticlesData.json");
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    _Articles = JsonConvert.DeserializeObject<List<XArticle>>(json);
                }
            }
        }

        public XArticle Add(XArticle pValue)
        {
            _Articles.Add(pValue);
            return pValue;
        }

        public long Count()
        {
            return _Articles.Count;
        }

        public bool Delete(int pObjectID)
        {
            XArticle a = _Articles.FirstOrDefault(o => o.ID == pObjectID);
            if (a == null)
                return false;
            _Articles.Remove(a);
            return true;
        }

        public IEnumerable<XArticle> Get(int pLimit, int pStart)
        {
            return _Articles.OrderBy(o => o.Title).Skip(pStart).Take(pLimit);
        }

        public XArticle GetObjectByID(int pObjectID)
        {
            return _Articles.FirstOrDefault(o => o.ID == pObjectID);
        }

        public XArticle Update(XArticle pValue)
        {
            if (!Delete(pValue.ID))
                return null;
            _Articles.Add(pValue);
            return pValue;
        }
    }
}