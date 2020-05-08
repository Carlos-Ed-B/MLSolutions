using Microsoft.ML.Data;

namespace ML.Modes.Mushrooms
{
    public class MushroomsPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Prediction { get; set; }
        [NoColumn]
        public string Description { get; set; }
        public float[] Score { get; set; }

    }
}
