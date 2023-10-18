using Monitoring.Common.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Common
{
    public class HttpManager
    {
        public async Task<T> GetRequest<T>(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.ConnectionClose = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12
               | SecurityProtocolType.Ssl3;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<T>(responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(uri + System.Environment.NewLine + ex.Message + ex.StackTrace+ex.InnerException.Message+ex.InnerException.StackTrace);
            }

            return default(T);
        }

        public async Task<dynamic> PostRequest<TIn>(string uri, TIn content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.ConnectionClose = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12
               | SecurityProtocolType.Ssl3;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.PostAsync(uri, serialized).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<dynamic>(responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(uri + System.Environment.NewLine + ex.Message + ex.StackTrace);
            }

            return default(dynamic);
        }

        public static string GetWebServiceBaseUrl()
        {
            string baseurl = string.Empty;
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\WebServiceUrl.txt";

                if (File.Exists(path))
                {
                    baseurl = File.ReadAllText(path);

                }
                else
                {
                    string domainalias = ConfigurationManager.AppSettings["DnsAlias"].ToString();
                    baseurl = domainalias + System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                }

            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }

            return baseurl;
        }
    }
}
