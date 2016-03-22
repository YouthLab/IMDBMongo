using System.Collections.Generic;

namespace DataModel.InputFileProcessing
{
    public class Business : MovieBase
    {
        public Business()
        {
            BoxOfficeGross = new List<string>();
            OpeningWeekend = new List<string>();
            Admissions = new List<string>();
            Studios = new List<string>();
        }

        public string Budget { get; set; }
        public List<string> BoxOfficeGross { get; set; }
        public List<string> OpeningWeekend { get; set; }
        public string Rental { get; set; }
        public List<string> Admissions { get; set; }
        public string ShootingDates { get; set; }
        public string ProductionDates { get; set; }
        public List<string> Studios { get; set; }
        public string CopyrightHolder { get; set; }
    }
}