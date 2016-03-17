using System.Collections.Generic;

namespace DataModel
{
    public class Certificate : MovieBase
    {
        public Certificate()
        {
            RatingList = new List<Ratings>();
        }

        public List<Ratings> RatingList { get; set; }
    }

    public class Ratings
    {
        public string Country { get; set; }
        public string Rating { get; set; }
    }
}