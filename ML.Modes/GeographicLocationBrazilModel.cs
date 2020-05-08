using CsvHelper.Configuration.Attributes;

namespace ML.Modes
{
    public class GeographicLocationBrazilModel
    {
        [Name("UF")]
        public string UF { get; set; }
        [Name("Municipio")]
        public string Municipio { get; set; }
    }
}
