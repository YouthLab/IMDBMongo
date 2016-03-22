using Anotar.NLog;
using DataModel;
using DataModel.InputFileProcessing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrepareInputFiles.Parsers
{
    public class ProdDesignerParser : FileParser
    {
        private Dictionary<string, ProductionDesigner> ProductionDesigners { get; set; }
        private ProductionDesigner _record;

        public ProdDesignerParser(string sourceFile)
        {
            SourceFile = sourceFile;
            RegularList = new List<string>
            {
                @".*\t\w*.*\(.*\).*"
            };
            ProductionDesigners = new Dictionary<string, ProductionDesigner>();
            PreHeaderLine1 = "Name			Titles";
            PreHeaderLine2 = "----			------";
            _record = new ProductionDesigner();
        }

        public override bool ParseFile(string destinationFile)
        {
            LogTo.Debug("\n\tBeign Parsing file");
            ReadRecords();
            LogTo.Debug("\n\tEnd Parsing input file");
            var orderedPd = ProductionDesigners.Values.OrderBy(n => n.Name);
            File.WriteAllText(destinationFile, JsonConvert.SerializeObject(orderedPd, Formatting.Indented));
            LogTo.Debug("Output JSON to {0}", destinationFile);
            return ProductionDesigners.Any();
        }

        protected override void PopulateRecords(IEnumerable<Match> lines)
        {
            foreach (var line in lines)
            {
                var scan = new Regex(@"^\s\s*");
                var artistLine = scan.Match(UtfStr(line));

                if (!artistLine.Success) //new Artist
                {
                    if (!string.IsNullOrWhiteSpace(_record.Name))
                    {
                        var mb = _record.MovieBases.OrderBy(y => y.Year).ToList();
                        _record.MovieBases = mb;
                        if (ProductionDesigners.ContainsKey(_record.Name))
                        {
                            var oldRcd = ProductionDesigners.First(m => m.Key.Equals(_record.Name)).Value;
                            foreach (var movieBase in _record.MovieBases)
                            {
                                oldRcd.MovieBases.Add(movieBase);
                            }
                        }
                        else
                            ProductionDesigners.Add(_record.Name, _record);
                    }
                    _record = new ProductionDesigner();
                    var utfStr = UtfStr(line);
                    var fields = utfStr.Split('\t').Where(v => v != "").ToArray();
                    if (fields.Length > 0)
                        _record.Name = fields[0].Trim();
                    if (fields.Length >= 2)
                        AddMoiveDetails(fields[1], _record);
                }
                else
                    foreach (var field in UtfStr(line).Split('\t').Where(v => v != "").ToArray())
                        AddMoiveDetails(field, _record);
            }
        }

        private void AddMoiveDetails(string s, ProductionDesigner localRecord)
        {
            var mb = new MovieBase();
            FixMovieNames(mb, s);
            localRecord.MovieBases.Add(mb);
        }
    }
}