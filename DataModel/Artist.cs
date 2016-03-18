using System.Collections.Generic;

namespace DataModel
{
    public abstract class Artist
    {
        protected Artist()
        {
            MovieBases = new List<MovieBase>();
        }

        public string Name { get; set; }

        //MovieBase
        public List<MovieBase> MovieBases { get; set; }
    }
}