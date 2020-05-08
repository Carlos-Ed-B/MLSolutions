using Microsoft.ML.Data;

namespace ML.Modes.HighwayBrazilAccidents
{
    public class HighwayBrazilAccidentsData
    {
        [ColumnName("uf"), LoadColumn(0)]
        public string Uf { get; set; }


        [ColumnName("br"), LoadColumn(1)]
        public float Br { get; set; }


        [ColumnName("km"), LoadColumn(2)]
        public float Km { get; set; }


        [ColumnName("municipio"), LoadColumn(3)]
        public string Municipio { get; set; }


        [ColumnName("causa_acidente"), LoadColumn(4)]
        public string Causa_acidente { get; set; }


        [ColumnName("tipo_acidente"), LoadColumn(5)]
        public string Tipo_acidente { get; set; }


        [ColumnName("tipo_veiculo"), LoadColumn(6)]
        public string Tipo_veiculo { get; set; }


        [ColumnName("marca"), LoadColumn(7)]
        public string Marca { get; set; }


        [ColumnName("tipo_envolvido"), LoadColumn(8)]
        public string Tipo_envolvido { get; set; }


    }
}
