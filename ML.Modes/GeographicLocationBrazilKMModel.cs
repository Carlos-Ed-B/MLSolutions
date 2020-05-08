using CsvHelper.Configuration.Attributes;

namespace ML.Modes
{
    public class GeographicLocationBrazilKMModel
    {
        [Name("Municipio")]
        public string Municipio { get; set; }
        [Name("KM")]
        public string KM { get; set; }
    }
}
