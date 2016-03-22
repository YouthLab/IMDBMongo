using System.Collections.Generic;

namespace DataModel
{
    public class SoundMix : MovieBase
    {
        public List<string> SoundModeList { get; set; }

        public SoundMix()
        {
            SoundModeList = new List<string>();
        }
    }
}