using System.Collections.Generic;

namespace sampler_api.Models
{
    public class Menu : Unique
    {
        public Menu(string guid)
        {
            GUID = guid;
        }
        public List<Input> Inputs { get; set; }
    }


    public class Input : BaseElement
    {
        public Input()
        {

        }
        public List<Input> Inputs { get; set; }
        public float? Max { get; set; }
        public float? Min { get; set; }
        public float? Value { get; set; }
        public float? Step { get; set; }
    }

}