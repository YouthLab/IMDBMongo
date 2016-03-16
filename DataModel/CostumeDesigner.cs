using System.Collections.Generic;

namespace DataModel
{
    public class CostumeDesigner
    {
        public CostumeDesigner()
        {
            MovieBases = new List<MovieBase>();
        }

        public string Name { get; set; }

        //MovieBase
        public List<MovieBase> MovieBases { get; set; }
    }
}