using System.Collections.Generic;

namespace DataModel
{
    public class TagLine : MovieBase
    {
        public TagLine()
        {
            TagLines = new List<string>();
        }

        public List<string> TagLines { get; set; }
    }
}