using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace bkfunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            string sentiment = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "sentiment", true) == 0)
                .Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Set name to query string or body data
            name = name ?? data?.name;
            sentiment = sentiment ?? data?.sentiment;

            if (name == null || sentiment == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Name or sentiment missing");
            }
            else
            {
                double sentimentValue = double.Parse(sentiment);
               
               return sentimentValue > 0.49 ?
                req.CreateResponse(HttpStatusCode.OK, name + " is happy") :
                req.CreateResponse(HttpStatusCode.OK, name + " is not happy");
            }
            
            
                
        }
    }
}
