using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sampler_api.Helpers;
using sampler_api.Models;
using sampler_api.Services;

namespace sampler_api.Repositories
{
    public class GraphRepository : IGraphRepository
    {
        private readonly ISimulator Simulator;
        public GraphRepository(ISimulator simulator)
        {
            Simulator = simulator;
        }

        public async Task<List<Graph>> GetGraphs(string guid)
        {
            var simulateParams = GetInitParams(guid);
            Simulate simulation = await Simulator.GetSimulate(simulateParams);
            List<Graph> graphs = await GetGraphsConfig();
            graphs.SetGraphsData(simulation, simulateParams.GUID);
            return graphs;
        }

        public SimulateParams GetInitParams(string guid)
        {
            return new SimulateParams()
            {
                GUID = guid,
                Population = "1000000",
                Prev = "0.5",
                Tp = "444446",
                Fp = "276814",
                Fn = "172754",
                Tn = "340386",
            };
        }
        public async Task<List<Graph>> UpdateGraphs(SimulateParams simulateParams)
        {
            Simulate simulation = await Simulator.GetSimulate(simulateParams);
            List<Graph> graphs = await GetGraphsConfig();
            graphs.SetGraphsData(simulation, simulateParams.GUID);
            return graphs;
        }

        private async Task<List<Graph>> GetGraphsConfig()
        {
            using (StreamReader r = new StreamReader("configgraphs.json"))
            {
                string json = await r.ReadToEndAsync();
                List<Graph> graphs = JsonConvert.DeserializeObject<List<Graph>>(json);
                return graphs;
            }
        }

    }
}