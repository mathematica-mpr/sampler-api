using System.Collections.Generic;
using System.Reflection;
using sampler_api.Models;

namespace sampler_api.Helpers
{
    public static class ExtensionMethods
    {
        public static void SetGraphsData(this List<Graph> chapterGraphs, Simulate simulation, string guid)
        {
            chapterGraphs.ForEach(chapterGraph =>
            {
                if (chapterGraph.Graphs.Count == 0)
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
                    chapterGraph.Graphs.SetGraphsData(simulation, guid);
                }
            });

        }
    }
}