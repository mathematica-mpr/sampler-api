using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sampler_api.Helpers;
using sampler_api.Models;
using sampler_api.Services;

namespace sampler_api.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ISimulator Simulator;
        public ChapterRepository(ISimulator simulator)
        {
            Simulator = simulator;
        }

        public async Task<Chapter> GetInitChapter(int id)
        {
            Chapter chapter = await GetChapter(id);
            SimulateParams simParams = GetSimulateParams(chapter.Inputs);
            Simulate simulation = await Simulator.Run(simParams);
            chapter.Graphs = GetChapterGraphsData(chapter.Graphs, simulation);

            return chapter;
        }

        public async Task<Chapter> GetChapter(int id)
        {
            // TODO: this should be replaced when DynamoDB
            using (StreamReader r = new StreamReader("chapter.json"))
            {
                string json = await r.ReadToEndAsync();
                Chapter chapter = JsonConvert.DeserializeObject<Chapter>(json);
                return chapter;
            }
        }

        private List<ChapterGraph> GetChapterGraphsData(List<ChapterGraph> chapterGraphs, Simulate simulation)
        {
            chapterGraphs.ForEach(chapterGraph =>
            {
                if (chapterGraph.Graphs == null)
                {
                    PropertyInfo prop = simulation.GetType().GetProperty(chapterGraph.Name);
                    if (prop != null)
                    {
                        List<Coordinate> rawData = (List<Coordinate>)prop.GetValue(simulation);
                        chapterGraph.Data = rawData;
                    }
                }
                else
                {
                    chapterGraph.Graphs = GetChapterGraphsData(chapterGraph.Graphs, simulation);
                }
            });

            return chapterGraphs;
        }

        private SimulateParams GetSimulateParams(List<ChapterInput> chapterInputs)
        {
            SimulateParams parentParams = new SimulateParams();
            SimulateParams childParams = new SimulateParams();

            chapterInputs.ForEach(chapterInput =>
            {
                if (chapterInput.Inputs == null)
                {
                    PropertyInfo prop = parentParams.GetType().GetProperty(chapterInput.Name);
                    prop.SetValue(parentParams, chapterInput.Init.ToString());
                }
                else
                {
                    childParams = GetSimulateParams(chapterInput.Inputs);
                }
            });

            SimulateParams combineParams = Utils.Combine<SimulateParams>(parentParams, childParams);
            return combineParams;
        }

        public async Task<Chapter> GetUpdatedChapter(int id, SimulateParams simulateParams)
        {
            Chapter chapter = await GetChapter(id);
            Simulate simulation = await Simulator.Run(simulateParams);
            chapter.Graphs = GetChapterGraphsData(chapter.Graphs, simulation);
            return chapter;
        }

    }
}