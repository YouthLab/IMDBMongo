using System.Collections.Generic;

namespace DataModel.InputFileProcessing
{
    public class CrazyCredits : MovieBase
    {
        public CrazyCredits()
        {
            Credits = new List<string>();
        }

        public List<string> Credits { get; set; }
    }
}