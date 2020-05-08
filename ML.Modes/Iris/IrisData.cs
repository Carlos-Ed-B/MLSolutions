using Microsoft.ML.Data;

namespace ML.Modes.Iris
{
    public class IrisData
    {

        [ColumnName("Label"), LoadColumn(0)]
        public float Label { get; set; }

        [ColumnName("Sepal length"), LoadColumn(1)]
        public float Sepal_length { get; set; }

        [ColumnName("Sepal width"), LoadColumn(2)]
        public float Sepal_width { get; set; }

        [ColumnName("Petal length"), LoadColumn(3)]
        public float Petal_length { get; set; }

        [ColumnName("Petal width"), LoadColumn(4)]
        public float Petal_width { get; set; }
    }
}
