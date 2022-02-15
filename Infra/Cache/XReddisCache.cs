using System;
using ServiceStack.Redis;
using Microsoft.Extensions.Configuration;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Cache;
using Microsoft.Extensions.Logging;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infra.Cache
{
    public sealed class XReddisCache : XBaseCache
    {
        public XReddisCache(IConfiguration pConfiguration, ILogger<XReddisCache> pLogger)
        {
            _Host = pConfiguration.GetValue<string>("Cache:Host");
            _Password = pConfiguration.GetValue<string>("Cache:Password");
            _Port = pConfiguration.GetValue("Cache:Port", "12220");
            string exp = pConfiguration.GetValue("Cache:ExpirationMilliseconds", "60000");
            _Logger = pLogger;
            int expiration;
            if (!int.TryParse(exp, out expiration))
                expiration = 60000;
            _Expiration = TimeSpan.FromMilliseconds(expiration);
        }

        private readonly string _Host;
        private readonly string _Port;
        private readonly string _Password;
        private readonly TimeSpan _Expiration;
        private readonly ILogger<XReddisCache> _Logger;
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
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Não foi possível remover a key \"{pKey}\" do Redis.");
            }
        }

        public override void Set(string pKey, object pObject)
        {
            try
            {
                var c = GetClient();
                c.Set(pKey, pObject, _Expiration);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Não foi possível escrever o key \"{pKey}\" no Redis.");
            }
        }

        protected override void TryRemoveByPattern(string pPattern)
        {
            try
            {
                var c = GetClient();
                c.RemoveByRegex(pPattern);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Não foi possível remover o padrão \"{pPattern}\" do Redis.");
            }
        }

        protected override bool TryGetFromCache<T>(string pKey, out T pOutput)
        {
            pOutput = default(T);
            try
            {
                var c = GetClient();
                if (!_Client.ContainsKey(pKey))
                    return false;
                pOutput = _Client.Get<T>(pKey);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Não foi tengar pegar o key \"{pKey}\" do Redis.");
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