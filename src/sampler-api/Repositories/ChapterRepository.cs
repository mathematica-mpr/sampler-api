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
            string guid = Utils.GenerateGUID();
            InitChapterInputs(chapter.Inputs, guid);
            await InitChapterGraphs(chapter, guid);
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
        public void InitChapterInputs(List<ChapterInput> chapterInputs, string guid)
        {

            chapterInputs.ForEach(input =>
            {
                AssignGUIDS(input, guid);
            });
        }

        public async Task InitChapterGraphs(Chapter chapter, string guid)
        {
            Simulate simulation = await GetSimulate(chapter.Inputs, guid);
            SetChapterGraphsData(chapter.Graphs, simulation, guid);
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

        // TODO: probably move that to Simulate service
        private async Task<Simulate> GetSimulate(List<ChapterInput> chapterInputs, string guid)
        {
            SimulateParams simParams = GetSimulateParams(chapterInputs, guid);
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
                        List<GraphItem> rawData = new List<GraphItem>();
                        rawData.Add(new GraphItem()
                        {
                            Coordinates = (List<Coordinate>)prop.GetValue(simulation),
                            GUID = guid
                        });

                        // TODO: remove once figured out how to double xhr
                        rawData.Add(new GraphItem()
                        {
                            Coordinates = (List<Coordinate>)prop.GetValue(simulation),
                            GUID = guid
                        });

                        chapterGraph.GraphItems = rawData.ToList();
                    }
                }
                else
                {
                    SetChapterGraphsData(chapterGraph.Graphs, simulation, guid);
                }
            });

        }

        private InputItem GetInputItem(ChapterInput chapterInput, string guid)
        {
            return chapterInput.InputItems.Where(x => x.GUID == guid).SingleOrDefault();
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
                    InputItem inputItem = GetInputItem(chapterInput, guid);
                    prop.SetValue(parentParams, inputItem.Value.ToString());
                }
                else
                {
                    childParams = GetSimulateParams(chapterInput.Inputs, guid);
                }
            });

            SimulateParams combineParams = Utils.Combine<SimulateParams>(parentParams, childParams);

            return combineParams;
        }

        public async Task<Chapter> GetUpdatedChapter(int id, SimulateParams simulateParams)
        {
            Chapter chapter = await GetChapter();
            Simulate simulation = await Simulator.Run(simulateParams);
            // chapter.Graphs = GetChapterGraphsData(chapter.Graphs, simulation);
            return chapter;
        }

    }
}