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
            Chapter chapter = await GetChapter();
            List<ChapterInput> inputs = await InitChapterInputs();
            SimulateParams simParams = GetSimulateParams(chapter.Inputs);
            Simulate simulation = await Simulator.Run(simParams);
            chapter.Graphs = GetChapterGraphsData(chapter.Graphs, simulation);

            return chapter;
        }


        // TODO: add controler for adding a set of inputs
        public async Task<List<ChapterInput>> AddChapterInputs()
        {
            Chapter chapter = await GetChapter();
            string guid = Utils.GenerateGUID();
            chapter.Inputs.ForEach(input =>
            {
                AddChapterItem(input, guid);
            });

            return chapter.Inputs;
        }
        public async Task<List<ChapterInput>> InitChapterInputs()
        {
            Chapter chapter = await GetChapter();
            string guid = Utils.GenerateGUID();
            chapter.Inputs.ForEach(input =>
            {
                AssignGUIDS(input, guid);
            });

            return chapter.Inputs;
        }

        public void AddChapterItem(ChapterInput chapterInput, string guid)
        {

            if (chapterInput.InputItems.Count() > 0)
            {
                chapterInput.InputItems.Add(new InputItem(guid));

            }
            else
            {
                chapterInput.Inputs.ForEach(input =>
                {
                    AddChapterItem(input, guid);
                });
            }
        }

        public void AssignGUIDS(ChapterInput chapterInput, string guid)
        {
            if (chapterInput.InputItems.Count() > 0)
            {
                chapterInput.InputItems.ForEach(item =>
               {
                   item.GUID = guid;
               });

            }
            else
            {
                chapterInput.Inputs.ForEach(input =>
                {
                    AssignGUIDS(input, guid);
                });
            }
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

        private List<ChapterGraph> GetChapterGraphsData(List<ChapterGraph> chapterGraphs, Simulate simulation)
        {
            chapterGraphs.ForEach(chapterGraph =>
            {
                if (chapterGraph.Graphs == null)
                {
                    PropertyInfo prop = simulation.GetType().GetProperty(chapterGraph.Name);
                    if (prop != null)
                    {
                        List<GraphItem> rawData = new List<GraphItem>();
                        rawData.Add(new GraphItem()
                        {
                            Coordinates = (List<Coordinate>)prop.GetValue(simulation),
                            GUID = Utils.GenerateGUID()
                        });

                        // TODO: remove once figured out how to double xhr
                        rawData.Add(new GraphItem()
                        {
                            Coordinates = (List<Coordinate>)prop.GetValue(simulation),
                            GUID = Utils.GenerateGUID()
                        });

                        chapterGraph.GraphItems = rawData.ToList();
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
            // SimulateParams parentParams = new SimulateParams();
            // SimulateParams childParams = new SimulateParams();

            // chapterInputs.ForEach(chapterInput =>
            // {
            //     if (chapterInput.Inputs == null)
            //     {
            //         PropertyInfo prop = parentParams.GetType().GetProperty(chapterInput.Name);
            //         prop.SetValue(parentParams, chapterInput.Init.ToString());
            //     }
            //     else
            //     {
            //         childParams = GetSimulateParams(chapterInput.Inputs);
            //     }
            // });

            // SimulateParams combineParams = Utils.Combine<SimulateParams>(parentParams, childParams);
            // return combineParams;
            return null;
        }

        public async Task<Chapter> GetUpdatedChapter(int id, SimulateParams simulateParams)
        {
            Chapter chapter = await GetChapter();
            Simulate simulation = await Simulator.Run(simulateParams);
            chapter.Graphs = GetChapterGraphsData(chapter.Graphs, simulation);
            return chapter;
        }

    }
}