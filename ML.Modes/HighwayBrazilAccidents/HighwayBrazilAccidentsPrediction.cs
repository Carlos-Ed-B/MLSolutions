using Microsoft.ML.Data;

namespace ML.Modes.HighwayBrazilAccidents
{
    public class HighwayBrazilAccidentsPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Prediction { get; set; }
        public float[] Score { get; set; }
    }
}
