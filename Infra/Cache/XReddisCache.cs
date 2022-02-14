using System;
using ServiceStack.Redis;
using Microsoft.Extensions.Configuration;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Cache;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infra.Cache
{
    public sealed class XReddisCache : XBaseCache
    {
        public XReddisCache(IConfiguration pConfiguration)
        {
            _Host = pConfiguration.GetValue<string>("Cache:Host");
            _Password = pConfiguration.GetValue<string>("Cache:Password");
            _Port = pConfiguration.GetValue("Cache:Port", "12220");
            string exp = pConfiguration.GetValue("Cache:ExpirationMilliseconds", "10000");
            int expiration;
            if (!int.TryParse(exp, out expiration))
                expiration = 10000;
            _Expiration = TimeSpan.FromMilliseconds(expiration);
        }

        private readonly string _Host;
        private readonly string _Port;
        private readonly string _Password;
        private readonly TimeSpan _Expiration;
        private RedisClient _Client;

        public override void Dispose()
        {
            _Client.Dispose();
        }

        protected override void Remove(string pKey)
        {
            try
            {
                var c = GetClient();
                c.Remove(pKey);
            }
            catch
            {
                Console.WriteLine("ERRO AO REMOVER OBJETO NO REDIS");
            }
        }

        public override void Set(string pKey, object pObject)
        {
            try
            {
                var c = GetClient();
                c.Set(pKey, pObject, _Expiration);
            }
            catch
            {
                Console.WriteLine("ERRO AO ESCREVER OBJETO NO REDIS");
            }
        }

        protected override bool TryGetFromCache<T>(string pKey, out T pOutput)
        {
            pOutput = default(T);
            try
            {
                var c = GetClient();
                if (!_Client.ContainsKey(pKey))
                {
                    Console.WriteLine("Sem cache");
                    return false;
                }
                pOutput = _Client.Get<T>(pKey);
                Console.WriteLine("Do cache");
            }
            catch
            {
                Console.WriteLine("ERRO AO TENGAR PEGAR OBJETO NO REDIS");
            }
            return pOutput != null;
        }

        private RedisClient GetClient()
        {
            if (_Client != null)
                return _Client;
            int port;
            if (!int.TryParse(_Port, out port))
                port = 12220;
            _Client = new RedisClient(_Host, port, _Password);
            return _Client;
        }
    }
}