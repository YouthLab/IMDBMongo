using System.Collections.Generic;

namespace DataModel.InputFileProcessing
{
    public class AlternateVersion : MovieBase
    {
        public AlternateVersion()
        {
            AlternateList = new List<string>();
        }

        public List<string> AlternateList { get; set; }
    }
}