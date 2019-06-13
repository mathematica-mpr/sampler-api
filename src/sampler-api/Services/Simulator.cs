using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using sampler_api.Models;

namespace sampler_api.Services
{
    public class Simulator : ISimulator
    {
        public async Task<Simulate> Run(SimulateParams simulateParams)
        {
            using (var client = new AmazonLambdaClient(RegionEndpoint.USEast1))
            {
                var apirequest = new APIGatewayProxyRequest()
                {
                    QueryStringParameters = new Dictionary<string, string>()
                };

                foreach (PropertyInfo param in simulateParams.GetType().GetProperties())
                {

                    apirequest.QueryStringParameters.Add(param.Name, param.GetValue(simulateParams).ToString());

                }

                var request = new InvokeRequest
                {
                    FunctionName = "simulate",
                    Payload = JsonConvert.SerializeObject(apirequest)
                };

                var response = await client.InvokeAsync(request);

                string result;

                using (var sr = new StreamReader(response.Payload))
                {
                    result = sr.ReadToEnd();
                    SimulateHttpResponse httpResponse = JsonConvert.DeserializeObject<SimulateHttpResponse>(result);
                    Simulate simulation = JsonConvert.DeserializeObject<Simulate>(httpResponse.Body);
                    return simulation;
                }
            }
        }

    }
}