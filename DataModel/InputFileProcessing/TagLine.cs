using System.Collections.Generic;

namespace DataModel.InputFileProcessing
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