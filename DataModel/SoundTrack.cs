using System.Collections.Generic;

namespace DataModel
{
    public class SoundTrack : MovieBase
    {
        public SoundTrack()
        {
            Titles = new List<string>();
        }

        public List<string> Titles { get; set; }
        //dropping other details. The file has no standard format
    }
}