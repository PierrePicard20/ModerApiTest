using System;
using System.Collections.Generic;

namespace ModerApiTest.Utils
{
    public class JWTTokenBlackList
    {
        private static JWTTokenBlackList _instance = null;

        private readonly LinkedList<(DateTime, string)> _orderedToken = new LinkedList<(DateTime, string)>();
        private readonly Dictionary<string, DateTime> _token2Date = new Dictionary<string, DateTime>();
        private readonly HashSet<string> _tokenBlackList = new HashSet<string>();

        private JWTTokenBlackList()
        {
        }

        public bool IsInHttpRequestPipeline { get; set; } = false;

        public static JWTTokenBlackList Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JWTTokenBlackList();
                }
                return _instance;
            }
        }

        public void AddToken(string token, DateTime expire)
        {
            if (!IsInHttpRequestPipeline)
                return;

            // clean up expired token first
            while (_orderedToken.First!=null && _orderedToken.First.Value.Item1 < DateTime.UtcNow)
            {
                var expiredToken = _orderedToken.First.Value.Item2;
                _orderedToken.RemoveFirst();
                _token2Date.Remove(expiredToken);
                _tokenBlackList.Remove(expiredToken);
            }
            // add the new token then
            _token2Date.Add(token, expire);
            _orderedToken.AddLast((expire, token));
        }

        public bool IsBlackListed(string token)
        {
            return _tokenBlackList.Contains(token);
        }

        public void AddToBlackList(string token)
        {
            if (!IsInHttpRequestPipeline)
                return;

            _tokenBlackList.Add(token);
        }
    }
}
