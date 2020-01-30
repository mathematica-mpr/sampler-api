using System.Collections.Generic;

namespace sampler_api.Models
{
    public class Graph : BaseElement
    {
        public Graph()
        {
            GraphItems = new List<GraphItem>();
            Graphs = new List<Graph>();
        }
        public List<GraphItem> GraphItems { get; set; }
        public List<Graph> Graphs { get; set; }
    }

    public class GraphItem : Unique
    {
        public List<Coordinate> Coordinates { get; set; }
    }
}