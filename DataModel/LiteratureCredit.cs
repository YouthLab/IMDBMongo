using System.Collections.Generic;

namespace DataModel
{
    public class LiteratureCredit : MovieBase
    {
        public LiteratureCredit()
        {
            Contributions = new Dictionary<string, LitContribution>();
        }

        public Dictionary<string, LitContribution> Contributions { get; set; }
    }

    public class LitContribution
    {
        public LitContribution()
        {
            Details = new List<string>();
        }

        public string Name { get; set; }
        public List<string> Details { get; set; }
        public CreditedFor CreditedFor { get; set; }
    }

    public enum CreditedFor
    {
        Adaptation, //ADPT
        Book, //BOOK
        CriticalReview, //CRIT
        Essay, //ESSY
        Interview, //IVIW
        Novel, //NOVL
        ProdProtocol, //PROT
        Script, //SCRP
        Other //OTHR
    }
}