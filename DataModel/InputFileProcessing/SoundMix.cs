using System.Collections.Generic;

namespace DataModel.InputFileProcessing
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