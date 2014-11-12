using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace IKriv.Azure
{
    internal class Authenticator
    {
        const string MsDateHeader = "x-ms-date";
        const string MsVersion = "x-ms-version";

        private readonly string[] _authHeaders =
            {
               "Content-Encoding",
               "Content-Language",
               "Content-Length",
               "Content-MD5",
               "Content-Type",
               "Date",
               "If-Modified-Since",
               "If-Match",
               "If-None-Match",
               "If-Unmodified-Since",
               "Range"            
            };

        private readonly Credentials _credentials;

        private Authenticator(Credentials credentials)
        {
            _credentials = credentials;
        }

        public static void Authenticate(HttpWebRequest request, Credentials credentials)
        {
            new Authenticator(credentials).Authenticate(request);
        }

        private void Authenticate(HttpWebRequest request)
        {
            request.Headers.Add(MsDateHeader, DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture));
            request.Headers.Add(MsVersion, "2009-09-19");

            string authString = 
                String.Format("{0}\n{1}\n{2}\n{3}",
                request.Method.ToUpperInvariant(),
                String.Join("\n", _authHeaders.Select(h=>request.Headers[h] ?? "")),
                CanonicalizeMsHeaders(request.Headers),
                CanonicalizeResource(request.RequestUri));

            string signature;

            using (var hashAlgorithm = new HMACSHA256(_credentials.Key))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(authString);
                signature = Convert.ToBase64String(hashAlgorithm.ComputeHash(bytes));
            }

            request.Headers.Add("Authorization", String.Format("SharedKey {0}:{1}", _credentials.Account, signature ));
        }

        private static string CanonicalizeMsHeaders(WebHeaderCollection headers)
        {
            return String.Join("\n",
                headers.AllKeys
                    .Select(key=>key.ToLower())
                    .Where(key=>key.StartsWith("x-ms-"))
                    .GroupBy(key=>key).Select(group=>group.Key) // unique keys
                    .OrderBy(key=>key)
                    .Select(key=>Tuple.Create(key, CanonicalizeValue(headers.Get(key))))
                    .Select(pair=>pair.Item1 + ":" + pair.Item2));
        }

        private static string CanonicalizeValue(string headerValue)
        {
            return String.Join(" ", headerValue.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
        }

        private string CanonicalizeResource(Uri uri)
        {
            var resource = "/" + _credentials.Account + uri.AbsolutePath;

            if (uri.Query.Length > 0)
            {
                var query = uri.Query;
                if (query.StartsWith("?")) query = query.Substring(1);

                resource += "\n" +
                    String.Join("\n",
                        query
                            .Split('&')
                            .Select(queries => queries.Split(new[] { '=' }, 2))
                            .Select(q => new { Name = q[0].ToLower(), Value = q.Length > 1 ? q[1] : "" })
                            .GroupBy(q => q.Name)
                            .OrderBy(g => g.Key)
                            .Select(g =>
                                String.Format("{0}:{1}", HttpUtility.UrlDecode(g.Key),
                                    String.Join(",", g.Select(q => HttpUtility.UrlDecode(q.Value)))))
                        );
            }

            return resource;
        }

    }
}
