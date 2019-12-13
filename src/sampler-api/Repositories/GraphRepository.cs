using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        public async Task<List<Graph>> GetGraphs(Menu menu)
        {
            Simulate simulation = await Simulator.GetSimulate(menu);
            List<Graph> graphs = await GetGraphsConfig();
            graphs.SetGraphsData(simulation, menu.GUID);
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