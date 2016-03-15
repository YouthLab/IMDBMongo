using System.Collections.Generic;

namespace DataModel
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