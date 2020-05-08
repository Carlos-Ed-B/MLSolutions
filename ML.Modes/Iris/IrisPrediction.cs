using Microsoft.ML.Data;
using ML.Modes.Iris.Enums;
using System;

namespace ML.Modes.Iris
{
    public class IrisPrediction
    {
        [NoColumn]
        public IrisTypeEnum Type { get; set; }
        
        [ColumnName("PredictedLabel")]
        public Single Prediction { get; set; }

        public float[] Score { get; set; }
    }
}
