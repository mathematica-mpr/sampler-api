using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using sampler_api.Helpers;
using sampler_api.Models;

namespace sampler_api.Services
{
    public class Simulator : ISimulator
    {
        public async Task<Simulate> GetSimulate(SimulateParams simulateParams)
        {
            Simulate simulation = await Run(simulateParams);
            return simulation;
        }

        // TODO: Remove once proved useless
        private SimulateParams GetSimulateParams(List<Input> chapterInputs, string guid)
        {
            SimulateParams parentParams = new SimulateParams();
            SimulateParams childParams = new SimulateParams();

            chapterInputs.ForEach(chapterInput =>
            {
                if (chapterInput.Inputs == null)
                {
                    PropertyInfo prop = parentParams.GetType().GetProperty(chapterInput.Name);
                    prop.SetValue(parentParams, chapterInput.Value.ToString());
                }
                else
                {
                    childParams = GetSimulateParams(chapterInput.Inputs, guid);
                }
            });

            SimulateParams combineParams = Utils.Combine<SimulateParams>(parentParams, childParams);

            return combineParams;
        }


        private async Task<Simulate> Run(SimulateParams simulateParams)
        {
            using (var client = new AmazonLambdaClient(RegionEndpoint.USEast2))
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
                    FunctionName = "simulateTesting",
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