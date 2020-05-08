using ML.Modes.Iris;
using ML.Modes.Iris.Enums;
using ML.Trainings.Iris;
using Newtonsoft.Json;
using Xunit;

namespace ML.Test
{
    public class IrisTrainingTest : BaseTest
    {
        [Fact]
        public void DoTrain()
        {
            var irisTraining = new IrisTraining();
            var result = irisTraining.DoTrain();

            Assert.True(result);
        }

        [Fact]
        public void DoPredict()
        {
            var irisTraining = new IrisTraining();
            irisTraining.DoLoadModel();

            var irisSetosaData = new IrisData
            {
                Sepal_length = 5.0f,
                Sepal_width = 3.2f,
                Petal_length = 1.2f,
                Petal_width = 0.2f
            };

            string json = JsonConvert.SerializeObject(irisSetosaData);

            var irisSetosaDataResult = irisTraining.DoPredict(irisSetosaData);

            Assert.True(irisSetosaDataResult.Type == IrisTypeEnum.Setosa);

            var irisVersicolorData = new IrisData
            {
                Sepal_length = 6.0f,
                Sepal_width = 2.9f,
                Petal_length = 4.5f,
                Petal_width = 1.5f
            };

            var irisVersicolorDataResult = irisTraining.DoPredict(irisVersicolorData);

            Assert.True(irisVersicolorDataResult.Type == IrisTypeEnum.Versicolor);

            var irisVirginicaData = new IrisData
            {
                Sepal_length = 6.9f,
                Sepal_width = 3.1f,
                Petal_length = 5.4f,
                Petal_width = 2.1f
            };

            var irisVirginicaDataResult = irisTraining.DoPredict(irisVirginicaData);

            Assert.True(irisVirginicaDataResult.Type == IrisTypeEnum.Virginica);

        }
    }
}
