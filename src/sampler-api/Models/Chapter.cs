using System.Collections.Generic;
using sampler_api.Helpers;

namespace sampler_api.Models
{

    public class ChapterElement
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }

    public class Chapter : ChapterElement
    {
        public List<ChapterElement> Descriptions { get; set; }
        public List<ChapterInput> Inputs { get; set; }
        public List<ChapterGraph> Graphs { get; set; }
    }

    public class ChapterInput : ChapterElement
    {
        public ChapterInput()
        {
            InputItems = new List<InputItem>();
        }
        public List<ChapterInput> Inputs { get; set; }
        public float? Max { get; set; }
        public float? Min { get; set; }
        public List<InputItem> InputItems { get; set; }
        public float? Step { get; set; }
    }


    public class ChapterItem
    {
        public string GUID { get; set; }
    }

    public class InputItem : ChapterItem
    {
        public InputItem(string guid)
        {
            GUID = guid;
        }
        public float? Value { get; set; }
    }

    public class ChapterGraph : ChapterElement
    {
        public ChapterGraph()
        {
            GraphItems = new List<GraphItem>();
        }
        public List<GraphItem> GraphItems { get; set; }
        public List<ChapterGraph> Graphs { get; set; }
    }

    public class GraphItem : ChapterItem
    {
        public List<Coordinate> Coordinates { get; set; }
    }
}