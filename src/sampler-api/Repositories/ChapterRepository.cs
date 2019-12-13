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

        public async Task<Chapter> GetInitChapter()
        {
            Chapter chapter = await GetChapter();

            var menus = chapter.Menus;

            for (int i = 0; i < menus.Count(); i++)
            {
                menus[i].GUID = Utils.GenerateGUID();
                await InitChapterGraphs(menus[i], chapter.Graphs);
            }

            return chapter;
        }

        public async Task InitChapterGraphs(Menu menu, List<ChapterGraph> chapterGraphs)
        {
            Simulate simulation = await GetSimulate(menu);
            SetChapterGraphsData(chapterGraphs, simulation, menu.GUID);
        }

        public async Task<Chapter> GetChapter()
        {
            // TODO: this should be replaced when DynamoDB
            using (StreamReader r = new StreamReader("chapter.json"))
            {
                string json = await r.ReadToEndAsync();
                Chapter chapter = JsonConvert.DeserializeObject<Chapter>(json);
                return chapter;
            }
        }

        // TODO: probably move that to Simulate service
        private async Task<Simulate> GetSimulate(Menu menu)
        {
            SimulateParams simParams = GetSimulateParams(menu.Inputs, menu.GUID);
            Simulate simulation = await Simulator.Run(simParams);

            return simulation;
        }

        private void SetChapterGraphsData(List<ChapterGraph> chapterGraphs, Simulate simulation, string guid)
        {
            chapterGraphs.ForEach(chapterGraph =>
            {
                if (chapterGraph.Graphs == null)
                {
                    PropertyInfo prop = simulation.GetType().GetProperty(chapterGraph.Name);
                    if (prop != null)
                    {
                        GraphItem newGraphItem = new GraphItem()
                        {
                            Coordinates = (List<Coordinate>)prop.GetValue(simulation),
                            GUID = guid
                        };

                        chapterGraph.GraphItems.Add(newGraphItem);
                    }
                }
                else
                {
                    SetChapterGraphsData(chapterGraph.Graphs, simulation, guid);
                }
            });

        }


        private SimulateParams GetSimulateParams(List<ChapterInput> chapterInputs, string guid)
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

        public async Task<List<ChapterGraph>> UpdateGraphs(SimulateParams simulateParams, List<ChapterGraph> chapterGraphs)
        {

            Simulate simulation = await Simulator.Run(simulateParams);
            // chapter.Graphs = GetChapterGraphsData(chapter.Graphs, simulation);
            // return chapter;

            return null;
        }

    }
}