using CsvHelper.Configuration.Attributes;

namespace ML.Modes
{
    public class VehicleModel
    {
        [Name("Marca")]
        public string Marca { get; set; }
        [Name("TipoVeiculo")]
        public string TipoVeiculo { get; set; }
    }
}
